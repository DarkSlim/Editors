function AnimationBuilder::saveAsset(%this)
{
   if(!isObject($ANIMASSET)) return;
   
   $ANIMASSET.setAnimationCycle(ANMCycleToggle.getStateOn());
   $ANIMASSET.RandomStart = ANMRandStartToggle.getStateOn();
   
   %assetname = AssetDatabase.getAssetName($ANIMASSET.getAssetId());
   %finalassetname = AnimassetnameGUI.getText();
   %filename = "^MyAssets/assets/animations/" @ %finalassetname @ ".asset.taml";
   
   AssetDatabase.refreshAsset($ANIMASSET.getAssetId());
   TamlWrite($ANIMASSET, %filename);
   
   %def = ModuleDatabase.findModule(MyAssets,1);
   
   AssetDatabase.addDeclaredAsset(%def,%filename);
   
   %finalasset = AssetDatabase.acquireAsset("MyAssets:"@%assetname);
   
   AssetDatabase.renameDeclaredAsset(%finalasset.getAssetId(), "MyAssets:" @ AnimassetnameGUI.getText());
   
}

function AnimationBuilder::popnquit(%this)
{
     Canvas.popDialog(AB_Main_GUI);

//Cleanup   

   %scene = AnimPreviewSW.getScene();
   %scene.clear(true);
   %scene.delete();
   
   %scene = ImageAssetPreviewGUI.getScene();
   %scene.clear(true);
   %scene.delete();

   $ANIMASSET = 0;
   $ANIMSPRITE = 0;
   
   $IMGASSETSCENE = 0;
   PreviewFrames.deleteObjects();
   $CURRENTEXPFRAME = 0;
   $CURRENTEXTSPRITE = 0;   
   
   //Else the keypad won't work!
   activateDirectInput();
   $enableDirectInput = true;
}
