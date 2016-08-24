function ImageSlicer::saveasset(%this)
{
   echo("Saving Asset :" SPC $CURRENTASSET SPC $CURRENTASSET.getAssetId());
   echo("Explicit mode is set to" SPC TMPDATA_OBJ.ExplicitMode);
   
   $CURRENTASSET.setExplicitMode(TMPDATA_OBJ.ExplicitMode);
   
   if(TMPDATA_OBJ.ExplicitMode) 
   {
      %this.prepExplicitData();
   }
      
   echo($CURRENTASSET.getExplicitMode());

   $CURRENTASSET.setCellWidth(TMPDATA_OBJ.CellWidth);
   $CURRENTASSET.setCellHeight(TMPDATA_OBJ.CellHeight);
   $CURRENTASSET.setCellCountX(TMPDATA_OBJ.CellCountX);
   $CURRENTASSET.setCellCountY(TMPDATA_OBJ.CellCountY);
   $CURRENTASSET.setCellStrideX(TMPDATA_OBJ.CellStrideX);
   $CURRENTASSET.setCellStrideY(TMPDATA_OBJ.CellStrideY);
   $CURRENTASSET.setCellOffsetX(TMPDATA_OBJ.CellOffsetX);
   
   $CURRENTASSET.setCellOffsetY(TMPDATA_OBJ.CellOffsetY);

   %assetname = AssetDatabase.getAssetName($CURRENTASSET.getAssetId());
   %finalassetname = IMGAssetNameGUI.getText();
   %filename = "^MyAssets/assets/images/" @ %finalassetname @ ".asset.taml";
   AssetDatabase.refreshAsset($CURRENTASSET.getAssetId());
   
   TamlWrite($CURRENTASSET, %filename);
   
   %def = ModuleDatabase.findModule(MyAssets,1);
   AssetDatabase.addDeclaredAsset(%def,%filename);
   
   %finalasset = AssetDatabase.acquireAsset("MyAssets:"@%assetname);   

   AssetDatabase.renameDeclaredAsset(%finalasset.getAssetId(), "MyAssets:" @ %finalassetname );
   AssetDatabase.refreshAllAssets(true);
   
   Canvas.popDialog(ImageSlicer_MainGUI);

//Cleanup   
   AssetDatabase.releaseAsset($CURRENTASSET.getAssetId());
   $CURRENTASSET = 0;
   $IMGASSETSCENE.delete();
   TMPDATA_OBJ.delete();
   StackSet.deleteObjects();
   GhostFrames.clear();
}

function ImageSlicer::prepExplicitData(%this)
{
   //Loop through each ghostframe, add explicit frame
   %count = GhostFrames.getCount();

   %largearea = CURRENTSPRITE.getArea();
   
   for(%i=0;%i<%count;%i++)
   {
      %gframe = GhostFrames.getObject(%i);        
      %area = %gframe.getArea();
  
      %finalx = mAbs(%area.x-%largearea.x)/CURRENTSPRITE.getWidth();
      %finalx *= $CURRENTASSET.getImageWidth();
  
      %finaly = mAbs(getWord(%area,3)-getWord(%largearea,3))/CURRENTSPRITE.getHeight();
      %finaly *= $CURRENTASSET.getImageHeight();
  
      %finalw = (%gframe.getWidth()/CURRENTSPRITE.getWidth())*$CURRENTASSET.getImageWidth();
      %finalh = (%gframe.getHeight()/CURRENTSPRITE.getHeight())*$CURRENTASSET.getImageHeight();
      
      %name ="";
      %framename = %gframe.Framename;
      
      if(strlen(%framename) > 0)
      {
         %name = %framename;
      }      
      $CURRENTASSET.addExplicitCell(%finalx, %finaly, %finalw, %finalh, %name);
   }
}