function ParticleEditor::CopyParticleAsset(%this, %source_Asset, %dest_Asset)
{
%LifeMode = %source_Asset.getLifeMode();
%dest_Asset.setLifeMode(%LifeMode);
%LifeTime = %source_Asset.getLifetime();
%dest_Asset.setLifetime(%LifeTime);

%FieldCount = %source_Asset.getSelectableFieldCount();

for(%itr = 0; %itr < %FieldCount; %itr++)
{
   %FieldName = %source_Asset.getSelectableFieldName(%itr);
   %source_Asset.selectField(%FieldName);
   %dest_Asset.selectField(%FieldName);
   
   %KeyCount = %source_Asset.getDataKeyCount();
   
   for (%kitr = 0; %kitr < %KeyCount; %kitr++)
   {
   %Key = %source_Asset.getDataKey(%kitr);
      
   %dest_Asset.addDataKey(%Key._0, %Key._1);
   }
}   

//--------------

%EmitterCount = %source_Asset.getEmitterCount();

   for(%EmitterItr = 0; %EmitterItr < %EmitterCount; %EmitterItr++)
   {
      %SRCEmitter = %source_Asset.getEmitter(%EmitterItr);
      %newemi = new ParticleAssetEmitter();
      
//Fields, done by hand

   %EmiAnim = %SRCEmitter.getAnimation();

   if(%EmiAnim $= "")
   {
   %Emiimage = %SRCEmitter.getImage();
   %newemi.setImage(%Emiimage);   
   
   %EmiRandomimageFrame = %SRCEmitter.getRandomImageFrame();
   %newemi.setRandomImageFrame(%EmiRandomimageFrame);
   
   %EmiimageFrame = %SRCEmitter.getImageFrame();
   %newemi.setImageFrame(%EmiimageFrame);   
   }
   else %newemi.setAnimation(%EmiAnim);
   
//-------------------------------------------------------
      
   %EmiAlphaTest = %SRCEmitter.getAlphaTest();
   %newemi.setAlphaTest(%EmiAlphaTest);
   
   %EmiSRCBlend = %SRCEmitter.getSrcBlendFactor();
   %newemi.setSrcBlendFactor(%EmiSRCBlend);   
   
   %EmiDSTBlend = %SRCEmitter.getDstBlendFactor();
   %newemi.setDstBlendFactor(%EmiDSTBlend);   
   
   %EmiBlend = %SRCEmitter.getBlendMode();
   %newemi.setBlendMode(%EmiBlend);
   
   %EmiAttachPTE = %SRCEmitter.getAttachPositionToEmitter();
   %newemi.setAttachPositionToEmitter(%EmiAttachPTE);
   
   %EmiAttachRTE = %SRCEmitter.getAttachRotationToEmitter();
   %newemi.setAttachRotationToEmitter(%EmiAttachRTE);
   
   %EmiOldest = %SRCEmitter.getOldestInFront();
   %newemi.setOldestInFront(%EmiOldest);
   
   %EmiIntense = %SRCEmitter.getIntenseParticles();
   %newemi.setIntenseParticles(%EmiIntense);
   
   %EmiSingle = %SRCEmitter.getSingleParticle();
   %newemi.setSingleParticle(%EmiSingle);
   
   %EmiLETR = %SRCEmitter.getLinkEmissionRotation();
   %newemi.setLinkEmissionRotation(%EmiLETR);
      
   
   %EmiName = %SRCEmitter.getEmitterName();
   %newemi.setEmitterName(%EmiName);
   
   %EmiType = %SRCEmitter.getEmitterType();
   %newemi.setEmitterType(%EmiType);
   
   %EmiAngle = %SRCEmitter.getEmitterAngle();
   %newemi.setEmitterAngle(%EmiAngle);
   
   %EmiOffset = %SRCEmitter.getEmitterOffset();
   %newemi.setEmitterOffset(%EmiOffset);
   
   %EmiSize = %SRCEmitter.getEmitterSize();
   %newemi.setEmitterSize(%EmiSize);
   
   %EmiFixedAspect = %SRCEmitter.getFixedAspect();
   %newemi.setFixedAspect(%EmiFixedAspect);
   
   %EmiFFA = %SRCEmitter.getFixedForceAngle();
   %newemi.setFixedForceAngle(%EmiFFA);
   
   %EmiOri = %SRCEmitter.getOrientationType();
   %newemi.setOrientationType(%EmiOri);
   
   %EmiKeepAlign = %SRCEmitter.getKeepAligned();
   %newemi.setKeepAligned(%EmiKeepAlign);
   
   %EmiAAO = %SRCEmitter.getAlignedAngleOffset();
   %newemi.setAlignedAngleOffset(%EmiAAO);
   
   %EmiRAO = %SRCEmitter.getRandomAngleOffset();
   %newemi.setRandomAngleOffset(%EmiRAO);
   
   %EmiR_A = %SRCEmitter.getRandomArc();
   %newemi.setRandomArc(%EmiR_A);
   
   %EmiFAO = %SRCEmitter.getFixedAngleOffset();
   %newemi.setFixedAngleOffset(%EmiFAO);
   
   %EmiPivot = %SRCEmitter.getPivotPoint();
   %newemi.setPivotPoint(%EmiPivot);
      
      
      %GraphCount = %SRCEmitter.getSelectableFieldCount();
      
      for(%GraphItr = 0; %GraphItr < %GraphCount; %GraphItr++)
      {
         %GraphName = %SRCEmitter.getSelectableFieldName(%GraphItr);
         
         %SRCEmitter.selectField(%GraphName);
         %newemi.selectField(%GraphName);
   
         %GraphKeyCount = %SRCEmitter.getDataKeyCount();
  
         for (%gkitr = 0; %gkitr < %GraphKeyCount; %gkitr++)
         {
         %Key = %SRCEmitter.getDataKey(%gkitr);
         %newemi.addDataKey(%Key._0, %Key._1);
         }
         
      }
      
   %dest_Asset.addEmitter(%newemi);
   }

TamlWrite(%dest_Asset, "test.asset.taml");
}

function ParticleEditor::SavePFX(%this)
{
%asset = AssetDatabase.acquireAsset(%this.PFX.Particle);

%savetarget = new ParticleAsset()
{
  AssetName = $PFXASSETNAME;
};

%this.CopyParticleAsset(%asset, %savetarget);

   %filename = "^MyAssets/assets/ParticleFX/" @ $PFXASSETNAME @ ".asset.taml";

   if($SuicideVictim)
   {
   %asset.setLifeMode("KILL");
   }
   
   %success = TamlWrite(%savetarget, %filename);
   
    
    if(%success)
      {
         echo("Succesfully saved" SPC %filename);
         
         if($SuicideVictim)
         {
         %asset.setLifeMode("STOP");
         }
         
      }

}

function ParticleEditor::popnquit(%this)
{
    Canvas.popDialog(PFX_Preview);
    activateDirectInput();
    $enableDirectInput = true;
}