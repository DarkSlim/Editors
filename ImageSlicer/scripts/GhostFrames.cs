function ImageSlicer::updateGhostFrames(%this)
{
 //need to scale this to displayed image size!
 
%count = GhostFrames.getCount();
%rc = 0;

for(%i=0; %i<%count; %i++)
{
 %obj = GhostFrames.getObject(%i);
 $IMGASSETSCENE.remove(%obj);
 %rc++;
}
GhostFrames.deleteObjects();

//We must always keep in mind that the sprite and the imageAsset do not have the same size!!!
%assetw = $CURRENTASSET.getImageWidth();
%asseth = $CURRENTASSET.getImageHeight();

%Spritew = (TMPDATA_OBJ.CellWidth/%assetw)*CURRENTSPRITE.getWidth();
%Spriteh = (TMPDATA_OBJ.CellHeight/%asseth)*CURRENTSPRITE.getHeight();
/*
%Spritew = CURRENTSPRITE.getWidth()/TMPDATA_OBJ.CellCountX;
%Spriteh = CURRENTSPRITE.getHeight()/TMPDATA_OBJ.CellCountY;
*/

%xpos = getWord(CURRENTSPRITE.getArea(), 0) + %Spritew/2;
%ypos = getWord(CURRENTSPRITE.getArea(), 3) - %Spriteh/2;

%stridexratio = TMPDATA_OBJ.CellStrideX/TMPDATA_OBJ.CellWidth;
%strideyratio = TMPDATA_OBJ.CellStrideY/TMPDATA_OBJ.CellHeight;
      
%celloffsetx_ratio = TMPDATA_OBJ.CellOffsetX/TMPDATA_OBJ.CellWidth;
%celloffsety_ratio = TMPDATA_OBJ.CellOffsetY/TMPDATA_OBJ.CellHeight;

%xpos += %celloffsetx_ratio*%Spritew;
%ypos -= %celloffsety_ratio*%Spriteh;

for(%y=0; %y<TMPDATA_OBJ.CellCountY; %y++)
{
for(%x=0; %x<TMPDATA_OBJ.CellCountX; %x++)
{
%tint = getRandomF(0.1,1.0);

%spr = new Sprite(){
    Image = "ImageSlicer:ExpGhost";
    Size = %Spritew SPC %Spriteh;
    Position = %xpos SPC %ypos;
    BlendColor = 0.8 SPC %tint SPC %tint;
 };
 %spr.setBlendAlpha(0.5);
 GhostFrames.add(%spr);
   
   $IMGASSETSCENE.add(%spr);
   
   if(TMPDATA_OBJ.CellStrideX!=0)
      %xpos += %Spritew*%stridexratio;
   else %xpos+=%Spritew;

}
%xpos = getWord(CURRENTSPRITE.getArea(), 0) + %Spritew/2;
%xpos += %celloffsetx_ratio*%Spritew;

if(TMPDATA_OBJ.CellStrideY!=0)
   %ypos -= %Spriteh*%strideyratio;
else %ypos-=%Spriteh;

}
}