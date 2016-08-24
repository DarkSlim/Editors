function ImageSlicer::createGhostFrameEXP(%this, %pos)
{
EXPDATA_OBJ.FrameCount+=1;

//Disappear or fade out current ghostframe
if(isObject($CURRENTEXTSPRITE))
{
   $CURRENTEXTSPRITE.setBlendAlpha(0.4);
}

$CURRENTEXPFRAME = EXPDATA_OBJ.FrameCount;
   
ExplicitList.addItem("Frame" SPC $CURRENTEXPFRAME,"0.8 0.8 0.8");
FrameNameGUI.setText("Frame" SPC $CURRENTEXPFRAME);
ExplicitList.setSelected($CURRENTEXPFRAME-1);

   %spr = new Sprite(){
   Image = "ImageSlicer:ExpGhost";
   SceneLayer = 0;
   Reference = $CURRENTEXPFRAME;
   Framename = "";
   };
   %spr.setBlendAlpha(0.7);
   %spr.setArea(%pos.x,%pos.y,%pos.x+1, %pos.y+1);
   GhostFrames.add(%spr);   
   $IMGASSETSCENE.add(%spr);
   
   $CURRENTEXTSPRITE = %spr;
   
%this.updateTXTEDT_EXP();
}