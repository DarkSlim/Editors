function EmitterPauseBTN::onAction(%this)
{
   %val = true;   
   if(ParticleEditor.PFX.getEmitterPaused(%this.EmitterIndex)) %val = false;
   
   ParticleEditor.PFX.setEmitterPaused(%val, %this.EmitterIndex);
}

function EmitterVisibleBTN::onAction(%this)
{
   if(ParticleEditor.PFX.getEmitterVisible(%this.EmitterIndex))  
      ParticleEditor.PFX.setEmitterVisible(0, %this.EmitterIndex);
   else
      ParticleEditor.PFX.setEmitterVisible(1, %this.EmitterIndex);
}

function LifeTimeSlider::onMouseDragged(%this)
{
   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %asset.setLifetime(%this.getValue());
   %this.Link.setText("LifeTime =" SPC mFloatLength(%this.getValue(),2));
}

function OriTypeCtrl::onSelect(%this, %id, %text)
{
   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   %Emitter.setOrientationType(%text);
}

function EmiTypeCtrl::onSelect(%this, %id, %text)
{
   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   
   %Emitter.setEmitterType(%text);
}



function LifeModeCtrl::onSelect(%this, %id, %text)
{   
   if(%text $= "KILL")
   {
      %text = "STOP";
      $SuicideVictim = true;
   }
   else $SuicideVictim = false;
      
   ParticleEditor.SetLifeMode(%text);
}

function BlendFactorCtrl::onSelect(%this, %id, %text)
{
%asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
%Emitter = %asset.getEmitter(%this.EmitterIndex);

if(%this._Type $= "src")
%Emitter.setSrcBlendFactor(%text);
else
%Emitter.setDstBlendFactor(%text);   
}


function AAOffsetCTRL::Act(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);
   %Emitter.setAlignedAngleOffset(%this.getText());
}
function AlphaTestCTRL::Act(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);
   %Emitter.setAlphaTest(%this.getText());
}

function NameEditCTRL::Act(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);
   %Emitter.setEmitterName(%this.getText());
}

function AssetNameEditCTRL::Act(%this)
{
 %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
 $PFXASSETNAME = %this.getText();
}

function EmiSizeCTRL::Act(%this)
{
   %val = %this.SizeType;

   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   %CurSize = %Emitter.getEmitterSize();
   
   switch(%val)
   {
      case 0 : //width
      
         if(%val<0)
         %val = 0;
         %Emitter.setEmitterSize(%this.getText() SPC %CurSize._1);
      
      case 1 : //height
         
         if(%val<0)
         %val = 0;
         %Emitter.setEmitterSize(%CurSize._0 SPC %this.getText());
   }
   
   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function OffSizeCTRL::Act(%this)
{
   %val = %this.SizeType;

   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   %CurOffset = %Emitter.getEmitterOffset();
   
   switch(%val)
   {
      case 0 : //width
      
         %Emitter.setEmitterOffset(%this.getText() SPC %CurOffset._1);
      
      case 1 : //height
         
         %Emitter.setEmitterOffset(%CurOffset._0 SPC %this.getText());
   }
   
   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function PivotCTRL::Act(%this)
{
   %val = %this.PivotType;

   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   %CurPivot = %Emitter.getPivotPoint();
   
   switch(%val)
   {
      case 0 : //x
      
         if(%val<0)
         %val = 0;
         %Emitter.setPivotPoint(%this.getText() SPC %CurPivot._1);
      
      case 1 : //y
         
         if(%val<0)
         %val = 0;
         %Emitter.setPivotPoint(%CurPivot._0 SPC %this.getText());
   }
   
   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}



function GuiTextEditCtrl::onTabComplete(%this)
{
   %this.Act();
}

function GuiTextEditCtrl::onReturn(%this)
{
      %this.Act();
}

function ConsoleEntry::Act(%this)
{
   //nothing, do.
}

function FixedForceAngleCTRL::Act(%this)
{
   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
     
   %Emitter.setFixedForceAngle(%this.getText());

   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}


function EmitAngleCTRL::Act(%this)
{
   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
     
   %Emitter.setEmitterAngle(%this.getText());

   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}


function FixedAngleOffsetCTRL::Act(%this)
{
   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   
   %Emitter.setFixedAngleOffset(%this.getText());

   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function RandomArcCTRL::Act(%this)
{
   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   
   %Emitter.setRandomArc(%this.getText());

   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function RandomAngleOffsetCTRL::Act(%this)
{
   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   
   %Emitter.setRandomAngleOffset(%this.getText());

   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function KACtrl::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);
   %state =  %Emitter.getKeepAligned();
   
   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setKeepAligned(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setKeepAligned(true);
   }
   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function AttachPosCtrl::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);

%state =  %Emitter.getAttachPositionToEmitter();
   
   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setAttachPositionToEmitter(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setAttachPositionToEmitter(true);
   }
  
      
       ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}


function FixedAngleCtrl::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);

%state =  %Emitter.getFixedAspect();
   
   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setFixedAspect(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setFixedAspect(true);
   }
  
      
       ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function IntensePFXCtrl::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);

%state =  %Emitter.getIntenseParticles();
   
   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setIntenseParticles(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setIntenseParticles(true);
   }
  
      
       ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function RandomFrameCtrl::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);

%state =  %Emitter.getRandomImageFrame();
   
   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setRandomImageFrame(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setRandomImageFrame(true);
   }
  
      
       ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}


function OldestPFXCtrl::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);
%state =  %Emitter.getOldestInFront();
   
   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setOldestInFront(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setOldestInFront(true);
   }

ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}
function SinglePFXCtrl::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);
%state =  %Emitter.getSingleParticle();
   
   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setSingleParticle(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setSingleParticle(true);
   }

ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function BlendModeCtrl::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);
%state =  %Emitter.getBlendMode();
   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setBlendMode(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setBlendMode(true);
   }

ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function LERCTRL::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);
   %state =  %Emitter.getLinkEmissionRotation();

   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setLinkEmissionRotation(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setLinkEmissionRotation(true);
   }

ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function AttPosCTRL::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);
   %state =  %Emitter.getAttachPositionToEmitter();
   
   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setAttachPositionToEmitter(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setAttachPositionToEmitter(true);
   }

ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function AttRotCTRL::onClick(%this)
{
   %Asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %Asset.getEmitter(%this.EmitterIndex);
   %state =  %Emitter.getAttachRotationToEmitter();
   
   if(%state)
   {
      %this.setStateOn(false);
      %Emitter.setAttachRotationToEmitter(false);
   }
   else
   {
      %this.setStateOn(true);
      %Emitter.setAttachRotationToEmitter(true);
   }

ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function MinMaxTextEdit::Act(%this)
{   
   %mode = %this.Identifier;
   %Link = %this.Link;
   
   switch$(%mode)
   {
      case "min": %curval = %this.getText();
                  %max = getWord(%Link.range,1);
                  
               if(%curval < %this.absmin || %curval > %max)
                  {
                  %this.setText(getWord(%Link.range,0));
                  return;
                  }
                  
                  %Link.range = %curval SPC %max;
                  
                  if(%this._Type)
                  {
                   %pfx = ParticleEditor.PFX;
                   %asset = AssetDatabase.acquireAsset(%pfx.Particle);
                   %Emitter = %asset.getEmitter(%Link.EmitterIndex);
                   %Emitter.selectField(%this.Field);
                   %Key = %asset.getDataKey(%this._KeyIndex);
                   %newval = %Emitter.getFieldValue(%Key._0);
                  }
                  else
                  {
                   %pfx = ParticleEditor.PFX;
                   %asset = AssetDatabase.acquireAsset(%pfx.Particle);
                   %asset.selectField(%this.Field);
                   %Key = %asset.getDataKey(%this._KeyIndex);
                   %newval = %asset.getFieldValue(%Key._0);
                  }
                   
                  %Link.setValue(%newval);
                  
      case "max": %curval = %this.getText();
                  %min = getWord(%Link.range,0);
                  
               if(%curval < %min || %curval > %this.absmax)
                  {
                  %this.setText(getWord(Link.range,1));
                  return;
                  }
                  
                  %Link.range = %min SPC %curval;
                  
                  if(%this._Type)
                  {
                   %pfx = ParticleEditor.PFX;
                   %asset = AssetDatabase.acquireAsset(%pfx.Particle);
                   %Emitter = %asset.getEmitter(%Link.EmitterIndex);
                   %Emitter.selectField(%this.Field);
                   %Key = %asset.getDataKey(%this._KeyIndex);
                   %newval = %Emitter.getFieldValue(%Key._0);
                  }
                  else
                  {
                   %pfx = ParticleEditor.PFX;
                   %asset = AssetDatabase.acquireAsset(%pfx.Particle);
                   %asset.selectField(%this.Field);
                   %Key = %asset.getDataKey(%this._KeyIndex);
                   %newval = %asset.getFieldValue(%Key._0);
                  }
                   
                  %Link.setValue(%newval);
                  
      
      case "val": %curval = %this.getText();
                  %min = getWord(%Link.range,0);
                  %max = getWord(%Link.range,1);
                  
                  if(%curval<%min)
                  {
                     %Link.setValue(%min);
                     %this.setText(%min);
                     return;
                  } else if(%curval>%max)                  
                     {
                     %Link.setValue(%max);
                     %this.setText(%max);
                     } else %Link.setValue(%curval);
                     
                     
                  if(%this._Type)
                  {
                   %pfx = ParticleEditor.PFX;
                   %asset = AssetDatabase.acquireAsset(%pfx.Particle);
                   %Emitter = %asset.getEmitter(%Link.EmitterIndex);
                   %Emitter.selectField(%this.Field);
                   %Emitter.setDataKeyValue(%this._KeyIndex,%curval);
                  }
                  else
                  {
                   %pfx = ParticleEditor.PFX;
                   %asset = AssetDatabase.acquireAsset(%pfx.Particle);
                   %asset.selectField(%this.Field);
                   %asset.setDataKeyValue(%this._KeyIndex,%curval);
                  }

   }
 
}

function EffectSlider::onMouseDragged(%this)
{
   %pfx = ParticleEditor.PFX;
   %asset = AssetDatabase.acquireAsset(%pfx.Particle);
   %asset.selectField(%this.Fieldname);   
   %asset.setDataKeyValue(%this._KeyIndex, %this.getValue());
   
   %this.ValCtrl.setText(mFloatLength(%this.getValue(),2));
}

function EmitterSlider::onMouseDragged(%this)
{
   %pfx = ParticleEditor.PFX;
   %asset = AssetDatabase.acquireAsset(%pfx.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   %Emitter.selectField(%this.Fieldname);   
   %Emitter.setDataKeyValue(%this._KeyIndex, %this.getValue());
   
   %this.ValCtrl.setText(mFloatLength(%this.getValue(),2));
}

function GuiRolloutCtrl::onExpanded(%this)
{
%newgui = %this;
%parent = %this.getParent();

while(%parent != PFX_Preview.getId())
{
   %newgui = %parent;
   %parent = %newgui.getParent();
   %class = %parent.getClassName();
   if(%class $= "GuiRolloutCtrl")
   {
      %parent.sizeToContents();
   }
}
}

function GuiRolloutCtrl::onCollapsed(%this)
{
%newgui = %this;
%parent = %this.getParent();

while(%parent != PFX_Preview.getId())
{
   %newgui = %parent;
   %parent = %newgui.getParent();
   %class = %parent.getClassName();
   if(%class $= "GuiRolloutCtrl")
   {
      %parent.sizeToContents();
   }
}
}

function ParticleEditor::ZoomToEffect(%this)
{
//This is the FOCUS
%pos = ParticleEditor.PFX.getPosition();
PFX_SW.setTargetCameraPosition(%pos._0, %pos._1);
PFX_SW.setTargetCameraZoom(1.0);
PFX_SW.startCameraMove(1.0);
}