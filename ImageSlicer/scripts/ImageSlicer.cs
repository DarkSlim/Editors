function ImageSlicer::imageslicerprep(%this, %asset)
{
   deactivateDirectInput();
   $enableDirectInput = false;
   
   $CURRENTASSET = %asset;
   $IMGASSETSCENE = %scene = new Scene();
      
   if(!isObject(CleanupAisle6)) new SimSet(CleanupAisle6);
   
   if(!isObject(TMPDATA_OBJ))
   {
   new ScriptObject(TMPDATA_OBJ)
   {
      ExplicitMode = false;
      FramesCount = 1;
      CellCountX = 1;
      CellCountY = 1;
      CellWidth = %asset.getImageWidth();
      CellHeight = %asset.getImageHeight();
      CellStrideX =0;
      CellStrideY =0;
      CellOffsetX =0;
      CellOffsetY =0;
      FilterMode = "NEAREST";
      CellRowOrder = 1;
   };
   }
   Canvas.pushDialog(ImageSlicer_MainGUI);
   
   new ScriptObject(SlicerListener);
   ImageAsset_SW.addInputListener(SlicerListener);

   IMGFilterToggleGUI.setStateOn(0);
   IMGCellrowOrderToggleGUI.setStateOn(1);

   SliceModeMenuGUI.clear();
   
   //Prepare UI
   SliceModeMenuGUI.add("Single Image", 0);
   SliceModeMenuGUI.add("Parameter Slice", 1);
   SliceModeMenuGUI.add("Explicit Mode", 2);
   
   SliceModeMenuGUI.setSelected(0);
      
   ImageAsset_SW.setScene(%scene);
   ImageAsset_SW.setCameraSize(80,80);
   ImageAsset_SW.setCameraZoom(1.0);
   ImageAsset_SW.setCameraPosition(0,0);

   %assetid = %asset.getAssetId();
   
   %size = calcaspect(%asset);
//Background!
   %spr = new Sprite(){
      Image = "ImageSlicer:ExpGhost";
      Size = %size;
      SceneLayer = 6;
      BlendColor = "0.0 1.0 0.0";
   };
   %spr.setBlendAlpha(0.4);
   %scene.add(%spr);
   
   %spr = new Sprite(CURRENTSPRITE){
      class = Dragon;
      UseInputEvents = true;
      Image = %assetid;
      Size = %size;
      SceneLayer = 5;
   };
   %scene.add(%spr);
   
   ImageSlicer.setupAssetStatsUI();
   
   ImageSlicer.updateTXTEDT();
}

function SliceModeMenuGUI::onSelect(%this, %id, %text)
{

if(!isObject(StackSet)) new SimSet(StackSet);
if(!isObject(GhostFrames)) new SimSet(GhostFrames);

%stackCount = StackSet.getCount();
%ghostframescount = GhostFrames.getCount();

if(isObject(DataEntry_Set))   
   DataEntry_Set.clear();
   else new SimSet(DataEntry_Set);

StackSet.deleteObjects();
IMGSlicerStack.updateStack();

for(%i=0; %i<%ghostframescount; %i++)
{
 %obj = GhostFrames.getObject(%i);
 $IMGASSETSCENE.remove(%obj);
}
GhostFrames.deleteObjects();
CleanupAisle6.deleteObjects();

if(%id==0)
{
//Single Frame
TMPDATA_OBJ.ExplicitMode = false;
return;
}

if(%id==1)
{
   TMPDATA_OBJ.ExplicitMode = false;
   
   ImageSlicer.prepstackParam();
}

if(%id==2)
{
   //Explicit
   TMPDATA_OBJ.ExplicitMode = true;
   if(!isObject(DataEntry_Set)) new SimSet(DataEntry_Set);
   ImageSlicer.prepstackExplicit();
}
}

function DataEntry_TB::Act(%this)
{    
   %val = %this.getText();   
   
   %assetwidth = $CURRENTASSET.getImageWidth();
   %assetheight = $CURRENTASSET.getImageHeight();
   
   switch$(%this.valtomod){
   case "CellCountX" :   
   
   //Cancels Cellstride cuz modifying cellstride will auto-generate CellcountX
   TMPDATA_OBJ.CellStrideX = 0;
   TMPDATA_OBJ.CellWidth = (%assetwidth-TMPDATA_OBJ.CellOffsetX)/%val;
   TMPDATA_OBJ.CellCountX = %val;

   case "CellCountY" :
   TMPDATA_OBJ.CellStrideY = 0;
   TMPDATA_OBJ.CellHeight = (%assetheight-TMPDATA_OBJ.CellOffsetY)/%val;  
   TMPDATA_OBJ.CellCountY = %val;
   
   case "CellWidth" :
 
   if(%val==0)
   {
      alxPlay("ToyAssets:denied");
      %this.setText(TMPDATA_OBJ.CellWidth);
      return;
   }
   
   if(TMPDATA_OBJ.CellStrideX!=0)
   {
      if(%val>TMPDATA_OBJ.CellStrideX)
      {
      alxPlay("ToyAssets:denied");
      %this.setText(TMPDATA_OBJ.CellWidth);
      return;
      }
   }
   if((%val*TMPDATA_OBJ.CellCountX)+TMPDATA_OBJ.CellOffsetX<=%assetwidth)
      TMPDATA_OBJ.CellWidth = %val;
   else{
      alxPlay("ToyAssets:denied");
      %this.setText(TMPDATA_OBJ.CellWidth);
      return;
   }
   
   case "CellHeight":
   
   if(%val==0)
   {
      alxPlay("ToyAssets:denied");
      %this.setText(TMPDATA_OBJ.CellHeight);
      return;
   }
   
   if(TMPDATA_OBJ.CellStrideY!=0)
   {
      if(%val>TMPDATA_OBJ.CellStrideY)
      {
      alxPlay("ToyAssets:denied");
      %this.setText(TMPDATA_OBJ.CellHeight);
      return;
      }
   }
   if((%val*TMPDATA_OBJ.CellCountY)+TMPDATA_OBJ.CellOffsetY<=%assetheight)
      TMPDATA_OBJ.CellHeight = %val;
   else{
      alxPlay("ToyAssets:denied");
      %this.setText(TMPDATA_OBJ.CellHeight);
      return;
   }
   
   case "CellStrideX":
   
   if(%val<TMPDATA_OBJ.CellWidth) %val+=TMPDATA_OBJ.CellWidth;
   
   if(%val>%assetwidth-TMPDATA_OBJ.CellOffsetX)
   {
      alxPlay("ToyAssets:denied");
      %this.setText(TMPDATA_OBJ.CellStrideX);
      return;
   }
   
   %tmp = mFloor((%assetwidth-TMPDATA_OBJ.CellOffsetX)/%val);

   TMPDATA_OBJ.CellCountX = %tmp;
   
   if(TMPDATA_OBJ.CellWidth == 0)
   {
    TMPDATA_OBJ.CellWidth = %val;  
   }
      
   TMPDATA_OBJ.CellStrideX = %val;
   
   case "CellStrideY":
      
     if(%val<TMPDATA_OBJ.CellHeight) %val+=TMPDATA_OBJ.CellHeight;
   
   if(%val>%assetheight-TMPDATA_OBJ.CellOffsetY)
   {
      alxPlay("ToyAssets:denied");
      %this.setText(TMPDATA_OBJ.CellStrideX);
      return;
   }
   
   %tmp = mFloor((%assetheight-TMPDATA_OBJ.CellOffsetY)/%val);

   TMPDATA_OBJ.CellCountY = %tmp;
   
   if(TMPDATA_OBJ.CellHeight == 0)
   {
    TMPDATA_OBJ.CellHeight = %val;  
   }
      
   TMPDATA_OBJ.CellStrideY = %val;
   
   case "CellOffsetX":
   
   if(TMPDATA_OBJ.CellCountX>0)
   {
      %tmp = mFloor((%assetwidth-%val)/TMPDATA_OBJ.CellWidth);
      
      if(%tmp < TMPDATA_OBJ.CellCountX)
         TMPDATA_OBJ.CellCountX = %tmp;
         
      TMPDATA_OBJ.CellOffsetX = %val;
      
   } else TMPDATA_OBJ.CellOffsetX = %val;  
      
   case "CellOffsetY":
   if(TMPDATA_OBJ.CellCountY>0)
   {
      %tmp = mFloor((%assetheight-%val)/TMPDATA_OBJ.CellHeight);
      if(%tmp < TMPDATA_OBJ.CellCountY)
         TMPDATA_OBJ.CellCountY = %tmp;
         
      TMPDATA_OBJ.CellOffsetY = %val;
   } else TMPDATA_OBJ.CellOffsetY = %val;
   
   }
   ImageSlicer.updateTXTEDT();
   ImageSlicer.updateGhostFrames();
}

function ImageSlicer::updateTXTEDT(%this)
{
%c = DataEntry_Set.getCount();

for(%i=0;%i<%c;%i++)
{
%obj = DataEntry_Set.getObject(%i);

switch$(%obj.valtomod){
   case "CellCountX" :    %obj.setText(TMPDATA_OBJ.CellCountX);
   case "CellCountY" :     %obj.setText(TMPDATA_OBJ.CellCountY);
   case "CellWidth" :   %obj.setText(TMPDATA_OBJ.CellWidth);
   case "CellHeight":   %obj.setText(TMPDATA_OBJ.CellHeight);
   case "CellStrideX":  %obj.setText(TMPDATA_OBJ.CellStrideX);
   case "CellStrideY":  %obj.setText(TMPDATA_OBJ.CellStrideY);
   case "CellOffsetX":  %obj.setText(TMPDATA_OBJ.CellOffsetX);
   case "CellOffsetY":  %obj.setText(TMPDATA_OBJ.CellOffsetY);
}
}
}

function IMGFilterToggleGUI::onClick(%this)
{
   if($CURRENTASSET.FilterMode $= "NEAREST")
   {
      $CURRENTASSET.setFilterMode("BILINEAR");
   }
   else if($CURRENTASSET.FilterMode $= "BILINEAR")
   {
      $CURRENTASSET.setFilterMode("NEAREST");
   }
}

function IMGCellrowOrderToggleGUI::onClick(%this)
{
   if($CURRENTASSET.CellRowOrder == 1)
   {
      $CURRENTASSET.setCellRowOrder(0);
   }
   else if($CURRENTASSET.CellRowOrder == 0)
   {
      $CURRENTASSET.setCellRowOrder(1);
   }
}

function calcaspect(%asset)
{
   %ori_w = %asset.getImageWidth();
   %ori_h = %asset.getImageHeight();
   
   if(%ori_h==%ori_w)
   {
       %ratio = 1;
       return(60 SPC 60);
   }
   
   if(%ori_h<%ori_w)
   {
   %ratio = %ori_h/%ori_w;
   %fin_w =60;
   %fin_h = %fin_w*%ratio;
   }
   else if(%ori_h>%ori_w)
   {
   %ratio = %ori_w/%ori_h;
   %fin_h = 60;
   %fin_w = %fin_h*%ratio;      
   }
   
   return %fin_w SPC %fin_h;
}