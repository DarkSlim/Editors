function ImageAsset_SW::onMouseWheelUp(%this, %lines)
{
   %zoom = %this.getCameraZoom();
   
   %targetzoom = %zoom+1;
   
   if(%targetzoom<4.0)
   {
      %this.setTargetCameraZoom(%targetzoom);
      %this.startCameraMove(0.2);
   }
}


function ImageAsset_SW::onMouseWheelDown(%this, %lines)
{
   %zoom = %this.getCameraZoom();
   
   %targetzoom = %zoom-1;
   
   if(%targetzoom>1.0)
   {
      %this.setTargetCameraZoom(%targetzoom);
      %this.startCameraMove(0.2);
   }
   else
   {
      %this.setTargetCameraZoom(1.0);
      %this.startCameraMove(0.2);
   }
}

function ImageAsset_SW::onRightMouseDown(%this, %whocares, %pos, %clicks)
{
   $IMGDRAG_PT = %pos;
}

function ImageAsset_SW::onRightMouseDragged(%this, %whocares, %pos, %clicks)
{
   
   %distancex = $IMGDRAG_PT.x - %pos.x;
   %distancey = $IMGDRAG_PT.y - %pos.y;
   
   %normvec = Vector2Normalize(%distancex SPC %distancey);

   %curpos = %this.getCameraPosition();
   %curzoom = %this.getCameraZoom();
   
   %mod = 1;
   
   %normvec.x *= %mod;
   %normvec.y *= %mod;
  
  %this.setCameraPosition(%curpos.x-%normvec.x, %curpos.y-%normvec.y);
   //%this.setTargetCameraPosition(%curpos.x-%normvec.x, %curpos.y-%normvec.y);
   
   //%this.startCameraMove(0.2);
   $IMGDRAG_PT = %pos;   
}

function ImageAsset_SW::onRightMouseUp(%this, %whatever, %pos, %clicks)
{
   $IMGDRAG_P=%pos;
}