function ImageSlicer::remexplicitframe(%this)
{
   //remove currently selected frame - ExplicitList
   %index = ExplicitList.getSelectedItem();
   ExplicitList.deleteItem(%index);
   echo("Removing frame" SPC %index+1);
   EXPDATA_OBJ.FrameCount--;
   
   %count = GhostFrames.getCount();
   %targetobj = NULL;
   
   for(%i=0;%i<%count;%i++)
   {
      %obj = GhostFrames.getObject(%i);
      
      if(%obj.Reference == (%index+1))
      {
         %targetobj = %obj;

      }
   }
   
   if(isObject(%targetobj))
   {
   %targetobj.removeFromScene();
   GhostFrames.remove(%targetobj);
   %targetobj.safedelete();
   }
   
   
   for(%i=0; %i<ExplicitList.getItemCount(); %i++)
   {
      
   //Shift GhostFrames accordingly!
   //If Reference is higher than that of the recently deleted, decrement Reference
   %count = GhostFrames.getCount();   
   for(%i=0;%i<%count;%i++)
   {
      %obj = GhostFrames.getObject(%i);
      if(%obj.Reference > (%index+1))
      {
         %obj.Reference--;
         %obj.Framename = ExplicitList.getItemText(%i+1);
      }
   }
   
   }
}

function DataEntryEXP_TB::Act(%this)
{    
   %val = %this.getText();
   
   %area = $CURRENTEXTSPRITE.getArea();
   %largearea = CURRENTSPRITE.getArea();
  
   %ratiow = CURRENTSPRITE.getWidth()/$CURRENTASSET.getImageWidth();
   %ratioh = CURRENTSPRITE.getHeight()/$CURRENTASSET.getImageHeight();
   
   switch$(%this.valtomod){
   case "X" :
   %finalx = (%val/$CURRENTASSET.getImageWidth())*CURRENTSPRITE.getWidth();
   %finalx += getWord(%largearea,0);
   $CURRENTEXTSPRITE.setArea(%finalx,getWord(%area,1),getWord(%area,2),getWord(%area,3));
   case "Y" :
   %val*=-1;
   %finaly = (%val/$CURRENTASSET.getImageHeight())*CURRENTSPRITE.getHeight();
   %finaly += getWord(%largearea,3);
   $CURRENTEXTSPRITE.setArea(getWord(%area,0),getWord(%area,1),getWord(%area,2),%finaly);
   case "X1" :
   %width = %ratiow*%val;
   %finalx = getWord(%area,0)+%width;
   $CURRENTEXTSPRITE.setArea(getWord(%area,0),getWord(%area,1),%finalx,getWord(%area,3));
   case "Y1" :
   %height = %ratioh*%val;
   %finaly = getWord(%area,3)-%height;
   $CURRENTEXTSPRITE.setArea(getWord(%area,0),%finaly,getWord(%area,2),getWord(%area,3));
   }
   ImageSlicer.updateTXTEDT_EXP();
}

function ImageSlicer::updateTXTEDT_EXP(%this)
{
  //Convert to Asset size!!!!
   
  %area = $CURRENTEXTSPRITE.getArea();
  %largearea = CURRENTSPRITE.getArea();
  
  %distx = mAbs(%area.x-%largearea.x)/CURRENTSPRITE.getWidth();
  %distx *= $CURRENTASSET.getImageWidth();
  
  %disty = mAbs(getWord(%area,3)-getWord(%largearea,3))/CURRENTSPRITE.getHeight();
  %disty *= $CURRENTASSET.getImageHeight();
  
  %assetw = ($CURRENTEXTSPRITE.getWidth()/CURRENTSPRITE.getWidth())*$CURRENTASSET.getImageWidth();
  %asseth = ($CURRENTEXTSPRITE.getHeight()/CURRENTSPRITE.getHeight())*$CURRENTASSET.getImageHeight();
  
   %count = DataEntry_Set.getCount();
   for(%i=0;%i<%count;%i++)
   {
      %obj = DataEntry_Set.getObject(%i);
      switch$(%obj.valtomod){
         case "X": %obj.setText(mRound(%distx));
         case "Y":%obj.setText(mRound(%disty));
         case "X1":%obj.setText(mRound(%assetw));
         case "Y1":%obj.setText(mRound(%asseth));
      }
   }
}

function IMGAssetNameGUI::Act(%this)
{
   //Do nothing
}

function FrameNameGUI::Act(%this)
{
if(!isObject(ExplicitList)) return;
//Change name of currently selected frame
%index = ExplicitList.getSelectedItem();
ExplicitList.setItemText(%index,%this.getText());

%count = GhostFrames.getCount();

$CURRENTEXTSPRITE.Framename = %this.getText();
}
