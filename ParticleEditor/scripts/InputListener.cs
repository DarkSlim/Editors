
function PFX_InputListener::onTouchDown(%this, %touchID, %worldPosition, %clicks)
{
 %this.LastMousepos_L = PFX_SW.getCanvasPoint(%worldPosition);
}

function PFX_InputListener::onTouchDragged(%this, %touchID, %worldPosition, %clicks)
{

%clickpos = PFX_SW.getCanvasPoint(%worldPosition);

%distance = "0 0";

%distance.x = %this.LastMousepos_L.x - %clickpos.x;
%distance.y = %this.LastMousepos_L.y - %clickpos.y;

%distance.x *= 0.35;
%distance.y *= 0.35;

%curpos = PFX_SW.getCameraPosition();

%curpos.x += %distance.x;
%curpos.y +=  %distance.y;

PFX_SW.setCameraPosition(%curpos.x, %curpos.y);
}

function PFX_InputListener::onRightMouseDown(%this, %touchID, %worldPosition, %clicks)
{
 %this.LastMousepos_R = PFX_SW.getCanvasPoint(%worldPosition);
}

function PFX_InputListener::onRightMouseDragged(%this, %touchID, %worldPosition, %clicks)
{

%clickpos = PFX_SW.getCanvasPoint(%worldPosition);

%distance = "0 0";

%distance.x = %this.LastMousepos_R.x - %clickpos.x;
%distance.y = %this.LastMousepos_R.y - %clickpos.y;

%distance.x *= 0.35;
%distance.y *= 0.35;

%curpos = PFX_SW.getCameraPosition();

%curpos.x += %distance.x;
%curpos.y +=  %distance.y;

PFX_SW.setCameraPosition(%curpos.x, %curpos.y);
}

function PFX_InputListener::onMouseWheelUp(%this, %lines)
{
  %zoom = PFX_SW.getCameraZoom();
   
   if(%zoom>1.0)
      %targetzoom = %zoom+1;
      else %targetzoom = %zoom + 0.15;
   
   
   if(%targetzoom<5.0)
   {
      PFX_SW.setTargetCameraZoom(%targetzoom);
      PFX_SW.startCameraMove(0.2);
   }
}

function PFX_InputListener::onMouseWheelDown(%this, %lines)
{
   %zoom = PFX_SW.getCameraZoom();
      
      if(%zoom<1.0)
         %targetzoom = %zoom-0.15;
         else %targetzoom = %zoom-1.0;
   
   if(%targetzoom>0.15)
   {
      PFX_SW.setTargetCameraZoom(%targetzoom);
      PFX_SW.startCameraMove(0.2);
   }
   else
   {
      PFX_SW.setTargetCameraZoom(0.15);
      PFX_SW.startCameraMove(0.2);
   }
}