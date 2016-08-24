
function AnimationBuilder::addframe(%this)
{

if(!isObject($ANIMASSET)) return;
   %listcount = ANMFrameList.getItemCount();

      //if a frame is currently selected in the list, add directly after
    %sel = ANMFrameList.getSelectedItem();
    echo("Currently selected list item is" SPC %sel);
    echo("Current Frame is" SPC AnimationBuilder.CurFrame);
        
    if((%sel != -1) && (%sel<%listcount-1))
    {
         ANMFrameList.insertItem(AnimationBuilder.CurFrame, %sel+1);
         ANMFrameList.setSelected(%sel+1,true);
    }
      else
      {
          ANMFrameList.addItem(AnimationBuilder.CurFrame);
          ANMFrameList.setSelected(%listcount,true);
      }

   AnimationBuilder.refreshanim();
}

function AnimationBuilder::duplicateframe(%this)
{
    %listcount = ANMFrameList.getItemCount();
    %sel = ANMFrameList.getSelectedItem();
    if(%sel==-1) return;
    
    %framenumber = ANMFrameList.getItemText(%sel);
      
      if(%sel<%listcount-1)
      {
         ANMFrameList.insertItem(%framenumber,%sel);
         ANMFrameList.setSelected(%sel+1,true);
      }
      else
      {
         ANMFrameList.addItem(%framenumber);
         ANMFrameList.setSelected(%listcount,true);
      }
      
      AnimationBuilder.refreshanim();
}

function AnimationBuilder::removeframe(%this)
{
    %listcount = ANMFrameList.getItemCount();
    %sel = ANMFrameList.getSelectedItem();
    
    if(%sel==-1)
    {
       echo("No frame selected!!!");
       return;
    }
    
    ANMFrameList.deleteItem(%sel);
    
    AnimationBuilder.refreshanim();
}

function AnimationBuilder::refreshanim(%this)
{
%count = ANMFrameList.getItemCount();
%frames = "0";

if(%count>0)
{
%frames = ANMFrameList.getItemText(0);

//Since we've already added 0
for(%i=1;%i<%count;%i++)
{
   %frames = %frames SPC ANMFrameList.getItemText(%i);
}
}

if(isObject($ANIMASSET))
{
   AssetDatabase.releaseAsset($ANIMASSET.getAssetId());
}

%this.createAnimAsset(%frames, AnimSpeedSlider.getValue());

//AssetDatabase.refreshAsset($ANIMASSET.getAssetId());

$ANIMSPRITE.playAnimation($ANIMASSET.getAssetId());
}