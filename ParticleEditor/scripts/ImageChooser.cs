function ImageChooser::onAction(%this)
{
ParticleEditor.PFXscene.setScenePause(true);
Canvas.pushDialog(GuiImageChooser);

if(!isObject(PFXDATA_OBJ)) new ScriptObject(PFXDATA_OBJ);
   PFXDATA_OBJ.Ctrl = %this;

if(!isObject(IMGChooserSW))
{
   
%topstack = new GuiStackControl(){
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         VertSizing = "top";
         Extent = "984 728";
         Position = "40 40";
};

//Left Pane ------------------------------------

%leftvertstack = new GuiStackControl(){
   StackingType = "Vertical";
   VertStacking = "Top to Bottom";
   Extent = "200 700";
};

%imagechooser = new GuiPopUpMenuCtrl(IMGCHS_POP){
   Profile = GuiPopUpMenuProfile;
   Extent = "160 40";
   VertSizing = "top";
};
%animchooser = new GuiPopUpMenuCtrl(ANIMCHS_POP){
   Profile = GuiPopUpMenuProfile;
   Extent = "160 40";
   VertSizing = "top";
};

%leftvertstack.add(%imagechooser);
%leftvertstack.add(%animchooser);

%framectrlstack = new GuiStackControl(){
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "200 36";
         };
         
%UpButton = new GuiBitmapButtonCtrl()
{
Command = "ParticleEditor.PreviewNextFrame();";
Profile = BlueButtonProfile;
Extent = "69 36";
buttonType = "PushButton";
bitmap = "^GuiModule/assets/gui/northArrow";
};
%framectrlstack.add(%UpButton);


%DnButton = new GuiBitmapButtonCtrl()
{
Command = "ParticleEditor.PreviewPrevFrame();";
Profile = BlueButtonProfile;
Extent = "69 36";
buttonType = "PushButton";
bitmap = "^GuiModule/assets/gui/southArrow";
};
%framectrlstack.add(%DnButton);

%framelabel= new GuiTextCtrl(FRLAB){
   text = "1/1";
   Extent = "38 36";
   };
%framectrlstack.add(%framelabel);

%leftvertstack.add(%framectrlstack);

%choiceStack = new GuiStackControl(){
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "200 40";
         };

%btn1 = new GuiButtonCtrl(){
   text = "Choose";
   Extent = "64 32";
   Profile = GreenButtonProfile;
   Command = "ParticleEditor.IMGCHS_CLOSE();";
};

%btn2 = new GuiButtonCtrl(){
   text = "Cancel";
   Extent = "64 32";
   Profile = GreenButtonProfile;
   Command = "ParticleEditor.IMGCHS_CANCEL();";
};

%choiceStack.add(%btn1);
%choiceStack.add(%btn2);

%leftvertstack.add(%choiceStack);
%leftvertstack.updateStack();

%topstack.add(%leftvertstack);
%topstack.updateStack();

IMGCHSWIN.add(%topstack);
//-----------------------------------------


%SW = new SceneWindow(IMGChooserSW){
   Extent = "600 600";
   BackgroundColor = "150 150 150 255";
   UseBackgroundColor = true;
   Position = "200 200";
   };
   %SW.setCameraZoom (1.0);
   %SW.setCameraSize(30, 30);
   
   %Scene = new Scene(IMGChooserScene);
   
   %SW.setScene(%Scene);

%topstack.add(%SW);

}

IMGChooserScene.clear(true);
IMGChooserScene.setScenePause(false);

%sprite = new Sprite(){
   Size = "20 20";
   };
IMGChooserScene.add(%sprite);

IMGChooserScene.mainsprite = %sprite;

if(%this.isAnim)
{
   %sprite.Animation = %this.Anim;
   FRLAB.setText("");
}else
{
   %sprite.setImage(%this.IMG);
   %curframe = %sprite.getImageFrame();
   %imgasset = AssetDatabase.acquireAsset(%sprite.getImage());
   %maxframe = %imgasset.getFrameCount();

   
FRLAB.setText(%curframe+1 @ "/" @ %maxframe);
}

}

function ParticleEditor::PreviewNextFrame(%this)
{

if(PFXDATA_OBJ.Ctrl.IsAnim) return;

%sprite = IMGChooserScene.mainsprite;
%curframe = %sprite.getImageFrame();
%imgasset = AssetDatabase.acquireAsset(%sprite.getImage());
%maxframe = %imgasset.getFrameCount()-1;

   if(%curframe+1 > %maxframe) %curframe = 0;
   else %curframe++;
   
FRLAB.setText(%curframe+1 @ "/" @ %maxframe+1);
   %sprite.setImageFrame(%curframe);
}

function ParticleEditor::PreviewPrevFrame(%this)
{

if(PFXDATA_OBJ.Ctrl.IsAnim) return;

%sprite = IMGChooserScene.mainsprite;
%curframe = %sprite.getImageFrame();
%imgasset = AssetDatabase.acquireAsset(%sprite.getImage());
%maxframe = %imgasset.getFrameCount()-1;

   if(%curframe-1 < 0) %curframe = %maxframe;
   else %curframe--;

FRLAB.setText(%curframe+1 @ "/" @ %maxframe+1);
   %sprite.setImageFrame(%curframe);
}

function IMGCHS_POP::onAdd(%this)
{
   %Query = new AssetQuery();
   
   AssetDatabase.findAssetType(%Query, ImageAsset, false);
   
   for(%itr = 0; %itr < %Query.getCount(); %itr++)
   {
      %Asset = AssetDatabase.acquireAsset(%Query.getAsset(%itr));
      %AssetID = %Asset.getAssetId();
      %module = AssetDatabase.getAssetModule(%AssetID);
   
      if(%module.ModuleId $= "MyAssets")
      {
      %this.add(%AssetID , %itr);
      }
}
}

function IMGCHS_POP::onSelect(%this, %id, %text)
{
   IMGChooserScene.mainsprite.setImage(%text);
   ANIMCHS_POP.setText("");
   %curframe = IMGChooserScene.mainsprite.getImageFrame();
   %imgasset = AssetDatabase.acquireAsset(IMGChooserScene.mainsprite.getImage());
   %maxframe = %imgasset.getFrameCount();
   
FRLAB.setText(%curframe+1 @ "/" @ %maxframe);
   PFXDATA_OBJ.Ctrl.IsAnim = false;
}


function ANIMCHS_POP::onAdd(%this)
{
   %Query = new AssetQuery();
   
   AssetDatabase.findAssetType(%Query, AnimationAsset, false);
   
   for(%itr = 0; %itr < %Query.getCount(); %itr++)
   {
      %Asset = AssetDatabase.acquireAsset(%Query.getAsset(%itr));
      %AssetID = %Asset.getAssetId();
      %module = AssetDatabase.getAssetModule(%AssetID);
   
      if(%module.ModuleId $= "MyAssets")
      {
      %this.add(%AssetID , %itr);
      }
}
}

function ANIMCHS_POP::onSelect(%this, %id, %text)
{
 FRLAB.setText("");
 IMGChooserScene.mainsprite.playAnimation(%text);
 IMGCHS_POP.setText("");
 PFXDATA_OBJ.Ctrl.IsAnim = true;
}

function ParticleEditor::IMGCHS_CLOSE(%this)
{
   IMGChooserScene.setScenePause(true);
   ParticleEditor.PFXscene.setScenePause(false);
   
   %sprite = IMGChooserScene.mainsprite;

   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle); 

   %Emitter = %asset.getEmitter(PFXDATA_OBJ.Ctrl.EmitterIndex);
   
   if(PFXDATA_OBJ.Ctrl.IsAnim)
   {
   PFXDATA_OBJ.Ctrl.Anim = %sprite.getAnimation();
   %anim_asset = AssetDatabase.acquireAsset(PFXDATA_OBJ.Ctrl.Anim);
   %ImageAssetID = %anim_asset.getImage();
   %Emitter.setAnimation(%anim_asset.getAssetId());   
   }
   else
   {
   PFXDATA_OBJ.Ctrl.IMG = %sprite.getImage();
   %frame = %sprite.getImageFrame();   
   %Emitter.setImage(%sprite.getImage());
   %Emitter.setImageFrame(%frame);
   }
   Canvas.popdialog(GuiImageChooser);
}

function ParticleEditor::IMGCHS_CANCEL(%this)
{
   IMGChooserScene.setScenePause(true);   
   Canvas.popdialog(GuiImageChooser);
   ParticleEditor.PFXscene.setScenePause(false);
}