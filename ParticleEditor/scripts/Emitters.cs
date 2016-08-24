function ParticleEditor::CreateNewEffect(%this)
{
   deactivateDirectInput();
   $enableDirectInput = false;
   
   %Loadedasset = TamlRead("../ParticleFX/defaultpfx.asset.taml");
   
   $PFXASSETNAME = %Loadedasset.AssetName;
   
   %asset = AssetDatabase.addPrivateAsset(%Loadedasset);

   %this.resetPFXEDITOR();
   
   %NewEff = new ParticlePlayer()
   {
      Particle = %asset;
   };
   
%this.isPaused = false;
%this.PFX = %NewEff;

%this.PFXscene.add(%NewEff);

%this.populateInspector(%NewEff, $PFXASSETNAME);

   %E_Count = %Loadedasset.getEmitterCount();
   
   for(%itr = 0; %itr < %E_Count; %itr++)
   {
   %Emitter = %Loadedasset.getEmitter(%itr);
	%Collapse = %this.populateEmittersInspector(%NewEff, %itr);
   %Collapse.callOnChildren("collapse");
   }
       %co = MainCategorySet.getCount();
   for(%itr=0;%itr<%co;%itr++)
   {
      %obj = MainCategorySet.getObject(%itr);
      echo(%obj);
      %obj.collapse();
   }
}

function ParticleEditor::LoadEffect(%this, %val)
{
   if(!%val) return;
   
   deactivateDirectInput();
   $enableDirectInput = false;
   
   %FileLoadDLG = new OpenFileDialog()
   {
      DefaultPath = "modules/Assets/MyAssets/assets/ParticleFX/";
      Title = "Load Particle Effect from file...";
      Filters = "*.asset.taml";      
      MustExist = true;      
      ChangePath = true;
   };

   %result = %FileLoadDLG.Execute();

if(%result)
   {
    %success = TamlRead(%FileLoadDLG.fileName);
    
    if(%success)
      {
      %this.resetPFXEDITOR();
      
      %Loadedasset = TamlRead(%FileLoadDLG.fileName);
          $PFXASSETNAME = %Loadedasset.AssetName;
      %lifeMode = %Loadedasset.getLifeMode();
      if(%lifeMode $= "KILL")
       {
          //Make sure that the emitter doesn't commit suicide, set this back when saving.
          %Loadedasset.setLifeMode("STOP");
          $SuicideVictim = true;
       }
      
      %asset = AssetDatabase.addPrivateAsset(%Loadedasset);
      
      %NewEffect = new ParticlePlayer()
      {
      Particle = %asset;      
      };

      %this.isPaused = false;
      %this.PFX = %NewEffect;

      %this.PFXscene.add(%NewEffect);

      %this.populateInspector(%NewEffect, $PFXASSETNAME);
   
    %E_Count = %Loadedasset.getEmitterCount();
   
   for(%itr = 0; %itr < %E_Count; %itr++)
   {
   %Emitter = %Loadedasset.getEmitter(%itr);
	%Collapse = %this.populateEmittersInspector(%NewEffect, %itr);
   %Collapse.callOnChildren("collapse");
   
   %co = MainCategorySet.getCount();
   for(%itr=0;%itr<%co;%itr++)
   {
      %obj = MainCategorySet.getObject(%itr);
      echo(%obj);
      %obj.collapse();
   }
  
   }
  }
   }
}

function AddEmiBTN::onAction(%this)
{
   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   
   %emi = new ParticleAssetEmitter()
   {
      EmitterName = "New Emitter";
      Image = "ToyAssets:hexagon";
   };
   
   %asset.addEmitter( %emi );
   
   %count = %asset.getEmitterCount();
   %NuIndex = %count - 1;
   
   echo("New Emitter Index :" SPC %NuIndex);
   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
    %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);

   ParticleEditor.populateEmittersInspector(ParticleEditor.PFX, %NuIndex);
       %co = MainCategorySet.getCount();
   for(%itr=0;%itr<%co;%itr++)
   {
      %obj = MainCategorySet.getObject(%itr);
      echo(%obj);
      %obj.collapse();
   }
}

function RemEmiBTN::onAction(%this)
{
   %asset = AssetDatabase.acquireAsset(ParticleEditor.PFX.Particle);
   %Emitter = %asset.getEmitter(%this.EmitterIndex);   
   
   %count = %asset.getEmitterCount();
   
   if(%count==1)
   {
      echo("Can't remove the only emitter!");
   }
   else
   {
      %asset.removeEmitter(%Emitter, false);
      ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
      
      ParticleEditor.schedule(0,KillEmitterUI, %this.Rollout, %asset);
   }  
   
}

function ParticleEditor::KillEmitterUI(%this, %GuiControl, %asset)
{
   %this.resetPFXEDITOR_KeepAsset(%asset);
}


