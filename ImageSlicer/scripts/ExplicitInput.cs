function ImageSlicer::isOutofbounds(%this,%quadcoords, %bounds)
{   
   %xresults = 0;
   %yresults = 0;
   
   %x = getWord(%quadcoords,0);
   %y = getWord(%quadcoords,1);//lower edge
   %x1 = getWord(%quadcoords,2);
   %y1 = getWord(%quadcoords,3);//top edge
   
   %tx = getWord(%bounds,0);
   %ty = getWord(%bounds,1);//lower edge
   %tx1 = getWord(%bounds,2);
   %ty1 = getWord(%bounds,3);//top edge

   if(%x<%tx) %xresults = "1";
   if(%x1>%tx1) %xresults = "2";
   if(%y<%ty) %yresults = "1";
   if(%y1>%ty1) %yresults = "2";
   
   return %xresults SPC %yresults;
}
//------------------------------------------------


//------------------------------------------------

function ExplicitListCtrl::onDoubleClick(%this)
{
   %index = ExplicitList.getSelectedItem();
   $CURRENTEXPFRAME = %index+1;

%count = GhostFrames.getCount();

if(isObject($CURRENTEXTSPRITE))
{
   %col = $CURRENTEXTSPRITE.getBlendColor(false);
   $CURRENTEXTSPRITE.fadeTo(%col.x SPC %col.y SPC %col.z SPC "0.4",4);
}

for(%i=0; %i<%count; %i++)
{   
   %obj = GhostFrames.getObject(%i);
   if(%obj.Reference == $CURRENTEXPFRAME)
   {
      $CURRENTEXTSPRITE = %obj;
      %col = $CURRENTEXTSPRITE.getBlendColor(false);
      $CURRENTEXTSPRITE.fadeTo(%col.x SPC %col.y SPC %col.z SPC "0.7",4);
   }
}
FrameNameGUI.setText(ExplicitList.getItemText(%index));

ImageSlicer.updateTXTEDT_EXP();
}

function CPick_class::onMouseDragged(%this)
{
   //r  g  b  a
     
   %col = %this.PickColor;
   
   if(getWordCount(%col)<2)
   {
      //This means we've got a colorname
      %col = getStockColorF(%this.PickColor);
   }
   
   %r = getWord(%col,0);
   %g = getWord(%col,1);
   %b = getWord(%col,2);
   
   %listitem = ExplicitList.getSelectedItem();
   ExplicitList.setItemColor(%listitem,%col.x SPC %col.y SPC %col.z);
   $CURRENTEXTSPRITE.fadeTo(%r SPC %g SPC %b SPC "0.7",1);
}

function ImageSlicer::popnquit(%this)
{
     Canvas.popDialog(ImageSlicer_MainGUI);

//Cleanup   
   AssetDatabase.releaseAsset($CURRENTASSET.getAssetId());
   $CURRENTASSET = 0;
   $IMGASSETSCENE.delete();
   TMPDATA_OBJ.delete();
   StackSet.deleteObjects();
   GhostFrames.clear();   
   activateDirectInput();
   $enableDirectInput = true;
}
