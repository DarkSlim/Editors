function Dragon::RemoteTouchDown(%this, %pos)
{
if(!TMPDATA_OBJ.ExplicitMode)return;

if($EXPLICITMODE==0)
{
   //Create!
   ImageSlicer.createGhostFrameEXP(%pos);
   $AnchorPoint = %pos;
}

if($EXPLICITMODE==1)
{
   //Determine where we've clicked
   %clickedobjects = $IMGASSETSCENE.pickPoint(%pos);

$FRAMEOFFSET = 0;
   for(%i=0;%i<%clickedobjects.count;%i++)
   {
      %obj = getWord(%clickedobjects,%i);
      if(%obj == $CURRENTEXTSPRITE)
      {
         $FRAMEOFFSET = %obj.getPositionX()-%pos.x SPC %obj.getPositionY()-%pos.y;
      }
   }

   if($FRAMEOFFSET==0)
   {
      //Start another frame!
      $EXPLICITMODE=0;
      ImageSlicer.createGhostFrameEXP(%pos);
      $AnchorPoint = %pos;
   }
}

}