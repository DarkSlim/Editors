function AnimationBuilder::LoadUI(%this)
{
deactivateDirectInput();
$enableDirectInput = false;

 Canvas.pushDialog(AB_Main_GUI);  
 %this.PopulateAssetList();

 if(!isObject(PreviewFrames)) new SimSet(PreviewFrames);
 
 %scn = new Scene();
 
 ImageAssetPreviewGUI.setScene(%scn);
 ImageAssetPreviewGUI.setCameraSize(60,60);  
 ImageAssetPreviewGUI.UseBackgroundColor = true;
 ImageAssetPreviewGUI.BackgroundColor = "0.25 0.5 0.5";
 
 %animscn = new Scene();
 AnimPreviewSW.setScene(%animscn);
 AnimPreviewSW.setCameraSize(60,60);
 AnimPreviewSW.UseBackgroundColor = true;
 AnimPreviewSW.BackgroundColor = "0.5 0.25 0.5";
 
 ANMCycleToggle.setStateOn(1);
 ANMRandStartToggle.setStateOn(0);   
   
 %spr = new Sprite();
 %animscn.add(%spr);

 AnimationBuilder.CurFrame = 0;
 AnimationBuilder.DisplayedAsset = "";
 
 $ANIMSPRITE = %spr;
 }

function ImageAssetSelectorGUI::onDoubleClick(%this)
{
    %scene = ImageAssetPreviewGUI.getScene();
   //cleanup!!!
   if(isObject(PreviewFrames))
   {
    %count = PreviewFrames.getCount();
    
    for(%itr=0;%itr<%count;%itr++)
    {
       %obj = PreviewFrames.getObject(%itr);
       %scene.remove(%obj);
    }
   PreviewFrames.deleteObjects();
   }

   //Acquire Asset, display frames
   %index = %this.getSelectedItem();
   %assetid = %this.getItemText(%index);
   %asset = AssetDatabase.acquireAsset(%assetid);
   
   %fcount = %asset.getFrameCount();
  
   %bufferx = 125;

for(%x=0;%x<%fcount;%x++)
{
      %framesize = %asset.getFrameSize(%curframe);
      
      //(Framewidth * SceneWindowWidth) / Assetimagewidth => finalsizewidth
      %finalsize = calcframeaspect(%framesize.x, %framesize.y, 60);
      
      %spr = new Sprite(){
      Image = %assetid;
      Frame = %x;
      Position = (%x*%finalsize.x)+%bufferx SPC 0;
      Size = %finalsize;
      };
      PreviewFrames.add(%spr);
      %scene.add(%spr);
}

%ff = PreviewFrames.getObject(0);
ImageAssetPreviewGUI.setCameraPosition(%ff.getPosition());
AnimationBuilder.DisplayedAsset=%asset;
AnimationBuilder.CurFrame = 0;
FrameonTotalLabel.setText("0" SPC "/" SPC %fcount-1);

AnimSpeedSlider.setValue(1.0);

ANMFrameList.clearItems();
AnimationBuilder.refreshanim();

$ANIMSPRITE.setSize(%finalsize.x SPC %finalsize.y);

//assetid name + ANM

AnimassetnameGUI.setText(AssetDatabase.getAssetName(%assetid) @ "_ANM");
}

function AnimassetnameGUI::Act(%this)
{
   if(!isObject($ANIMASSET)) return;
   AnimationBuilder.refreshanim();
}

function AnimationBuilder::PopulateAssetList(%this)
{
ImageAssetSelectorGUI.clearItems();
%query = new AssetQuery();
AssetDatabase.findAssetType(%query, ImageAsset);

%count = %query.getCount();

for(%itr=0; %itr<%count;%itr++)
{
   %assetid = %query.getAsset(%itr);
   %module = AssetDatabase.getAssetModule(%assetid);
   
   if(%module.ModuleId $= "MyAssets")
   {
     ImageAssetSelectorGUI.addItem(%assetid);
   }
}
}

function calcframeaspect(%w, %h, %targetsize)
{  
   if(%w==%h)
   {
       %ratio = 1;
       return(%targetsize SPC %targetsize);
   }
   
   if(%h<%w)
   {
   %ratio = %h/%w;
   %fin_w = %targetsize;
   %fin_h = %fin_w*%ratio;
   }
   else if(%h>%w)
   {
   %ratio = %w/%h;
   %fin_h = %targetsize;
   %fin_w = %fin_h*%ratio;      
   }
   
   return %fin_w SPC %fin_h;
}

function AnimationBuilder::nextframe(%this)
{
   if(!isObject(%this.DisplayedAsset)) return;
   %fcount = %this.DisplayedAsset.getFrameCount();
   if(AnimationBuilder.CurFrame+1 == %fcount)
   {
      AnimationBuilder.CurFrame=0;
   }
   else 
   {
      AnimationBuilder.CurFrame++;
   }

   %count = PreviewFrames.getCount();
   
   for(%itr=0;%itr<%count;%itr++)
   {
       %obj = PreviewFrames.getObject(%itr);
         if(%obj.getImageFrame() == AnimationBuilder.CurFrame)
         %targetsprite = %obj;
   }
   
   if(isObject(%targetsprite))
   {
      %tpos = %targetsprite.getPosition();      
      ImageAssetPreviewGUI.setTargetCameraPosition(%tpos);
      ImageAssetPreviewGUI.startCameraMove(0.3);
   }
   FrameonTotalLabel.setText(AnimationBuilder.CurFrame SPC "/" SPC %fcount-1);
}

function AnimationBuilder::prevframe(%this)
{
   if(!isObject(%this.DisplayedAsset)) return;
   
   %fcount = %this.DisplayedAsset.getFrameCount();
   
   if(AnimationBuilder.CurFrame == 0)
   {
      AnimationBuilder.CurFrame=%fcount-1;
   }
   else 
   {
      AnimationBuilder.CurFrame--;
   }

   %count = PreviewFrames.getCount();
   
   for(%itr=0;%itr<%count;%itr++)
   {
       %obj = PreviewFrames.getObject(%itr);
         if(%obj.getImageFrame() == AnimationBuilder.CurFrame)
         %targetsprite = %obj;
   }
   
   if(isObject(%targetsprite))
   {
      %tpos = %targetsprite.getPosition();      
      ImageAssetPreviewGUI.setTargetCameraPosition(%tpos);
      ImageAssetPreviewGUI.startCameraMove(0.3);
   }
   FrameonTotalLabel.setText(AnimationBuilder.CurFrame SPC "/" SPC %fcount-1);
}

function AnimationBuilder::createAnimAsset(%this, %frames, %Animspeed)
{
 %animasset = new AnimationAsset(){
    Image = %this.DisplayedAsset.getAssetId();
    AnimationFrames = %frames;
    AnimationTime = %Animspeed;
    AssetName = AnimassetnameGUI.getText();
    };
    %tmpasset = AssetDatabase.addPrivateAsset(%animasset);

    $ANIMASSET = %animasset;
}

function AnimSpeedSlider::onMouseDragged(%this)
{
   if(!isObject($ANIMASSET)) return;
   AnimationBuilder.refreshanim();
}

function AnimationBuilder::Dumpallframes(%this)
{
   %fcount = %this.DisplayedAsset.getFrameCount();
   ANMFrameList.clearItems();
   
   for(%itr=0; %itr<%fcount; %itr++)
   {
      ANMFrameList.addItem(%itr);
   }
   
   AnimationBuilder.refreshanim();
}

function AnimationBuilder::prevframeANM(%this)
{
  if(!isObject($ANIMASSET)) return;

   %index = ANMFrameList.getSelectedItem();
   %max = ANMFrameList.getItemCount();
   
   $ANIMSPRITE.setAnimationTimeScale(0.0);
   
   if((%index)==0) %curframe = %max-1;
   else %curframe=%index-1;
   
   ANMFrameList.setSelected(%curframe, true);

   $ANIMSPRITE.setAnimationFrame(%curframe);
}

function AnimationBuilder::nextframeANM(%this)
{
   if(!isObject($ANIMASSET)) return;

   %max = ANMFrameList.getItemCount();
   %index = ANMFrameList.getSelectedItem();

   $ANIMSPRITE.setAnimationTimeScale(0.0);
   
   if((%index+1)==%max) %curframe = 0;
   else %curframe=%index+1;
   
   ANMFrameList.setSelected(%curframe, true);

   $ANIMSPRITE.setAnimationFrame(%curframe);
}

function AnimationBuilder::playpauseANM(%this)
{
   if(!isObject($ANIMASSET)) return;
   %scale = $ANIMSPRITE.getAnimationTimeScale();
   if(%scale == 0.0) %nuscale = 1.0;
      else %nuscale = 0.0;

   $ANIMSPRITE.setAnimationTimeScale(%nuscale);
   
   %curframe = $ANIMSPRITE.getAnimationFrame();
   ANMFrameList.setSelected(%curframe, true);
}

function ANMFrameList::onDoubleClick(%this)
{
   if(!isObject($ANIMASSET)) return;
    
   %sel = %this.getSelectedItem();
   %frame = %this.getItemText(%sel);
   $ANIMSPRITE.setAnimationTimeScale(0.0);
   //should set the frame index, frame number will not work in animationasset
   $ANIMSPRITE.setAnimationFrame(%sel);
}