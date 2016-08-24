function SlicerListener::onTouchDown(%this, %id, %pos, %clicks)
{
%object = ImageSlicer.pickit(%pos);

if(!%object) return;
   %object.RemoteTouchDown(%pos);
}

function SlicerListener::onTouchDragged(%this, %id, %pos, %clicks)
{
%object = ImageSlicer.pickit(%pos);

if(!TMPDATA_OBJ.ExplicitMode)return;

if($EXPLICITMODE==0)
{
%lastarea = $CURRENTEXTSPRITE.getArea();

if($AnchorPoint.x < %pos.x)
{
 %new_areaminx = $AnchorPoint.x;
 %new_areamaxx = %pos.x;
}
else
{
 %new_areaminx = %pos.x;
 %new_areamaxx = $AnchorPoint.x;
}
if($AnchorPoint.y < %pos.y)
{
 %new_areaminy = $AnchorPoint.y;
 %new_areamaxy = %pos.y;
}
else
{
 %new_areaminy = %pos.y;
 %new_areamaxy = $AnchorPoint.y;
}
%CSArea = CURRENTSPRITE.getArea();
%outofbounds = ImageSlicer.isOutofbounds(%new_areaminx SPC %new_areaminy SPC %new_areamaxx SPC %new_areamaxy, %CSArea);

if(%outofbounds.x==1)
{
%new_areaminx = getWord(%CSArea,0);
} else if (%outofbounds.x==2) %new_areamaxx = getWord(%CSArea,2);

if(%outofbounds.y==1)
{
%new_areaminy = getWord(%CSArea,1);
} else if (%outofbounds.y==2) %new_areamaxy = getWord(%CSArea,3);

$CURRENTEXTSPRITE.setArea(%new_areaminx,%new_areaminy,%new_areamaxx,%new_areamaxy);
 ImageSlicer.updateTXTEDT_EXP();
 return;
}

if($EXPLICITMODE==1)
{
if($FRAMEOFFSET==0) return;

%lastarea = $CURRENTEXTSPRITE.getArea();

$CURRENTEXTSPRITE.setPositionX(%pos.x+$FRAMEOFFSET.x);
$CURRENTEXTSPRITE.setPositionY(%pos.y+$FRAMEOFFSET.y);

%nextarea = $CURRENTEXTSPRITE.getArea();
%CSArea = CURRENTSPRITE.getArea();

%hw = $CURRENTEXTSPRITE.getWidth()/2;
%hh = $CURRENTEXTSPRITE.getHeight()/2;

%posx = $CURRENTEXTSPRITE.getPositionX();
%posy = $CURRENTEXTSPRITE.getPositionY();

%outofbounds = ImageSlicer.isOutofbounds(%nextarea, %CSArea);

if(%outofbounds.x==1)
{
%posx = getWord(%CSArea,0)+%hw;
} else if (%outofbounds.x==2) %posx = getWord(%CSArea,2)-%hw;

if(%outofbounds.y==1)
{
%posy = getWord(%CSArea,1)+%hh;
} else if (%outofbounds.y==2) %posy = getWord(%CSArea,3)-%hh;

$CURRENTEXTSPRITE.setPosition(%posx, %posy);
   ImageSlicer.updateTXTEDT_EXP();
   return;
}
}

function SlicerListener::onTouchUp(%this, %id, %pos, %clicks)
{
$EXPLICITMODE=1;//Move on to Edit mode!

if(isObject($CURRENTEXTSPRITE))
{
   %w = $CURRENTEXTSPRITE.getWidth();
   %h = $CURRENTEXTSPRITE.getHeight();
   
   if(%w<1.0 || %h <1.0)
   {
       $CURRENTEXTSPRITE.removeFromScene();
       $CURRENTEXTSPRITE.safeDelete();
       return;
   }
   
   %col = $CURRENTEXTSPRITE.getBlendColor(false);
   $CURRENTEXTSPRITE.fadeTo(%col.x SPC %col.y SPC %col.z SPC "0.7",1);
}
}



function ImageSlicer::pickit(%this, %pos)
{
   %foundit = false;
   
   %picked = $IMGASSETSCENE.pickPoint(%pos);
   
   %count = getWordCount(%picked);

   for(%itr=0;%itr<%count;%itr++)
   {
      %obj = getWord(%picked, %itr);
      
      if(%obj == CURRENTSPRITE.getId()) return %obj;
   }


return 0;
}