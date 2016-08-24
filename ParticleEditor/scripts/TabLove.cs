function KeyTimeTXTED::Act(%this)
{

//First let's sort the data keys to determine where our new key will insert itself.

   %pfx = ParticleEditor.PFX;
   %asset = AssetDatabase.acquireAsset(%pfx.Particle);
   
   if(%this.isEmitter)
   {
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   %Emitter.selectField(%this.FieldName);
   
   %timelineNormalize = 0;
   
   if((%this.FieldName $= "RedChannel") || (%this.FieldName $= "BlueChannel") || (%this.FieldName $= "GreenChannel") || (%this.FieldName $= "AlphaChannel"))
   %timelineNormalize = 1;
   
   %type = %this.TimeLineType;
   if(%type == 2)
   %timelineNormalize = 1;   
   
   %value = %this.getText();

   if((%timelineNormalize) && ((%value < 0.0) || (%value > 1.0)))
   {
      echo("Valid Value range is 0.0 to 1.0. Stay Within the lines!");
      return;
   }
   
   %count = %Emitter.getDataKeyCount();
   if(%count==1)
   {
      echo("Base Key must stay at 0.0");
      return;
   }

   %key = %Emitter.getDataKey(%this.DataKeyIndex);
   %keyval =  %key._1;
   
   %Emitter.removeDataKey(%this.DataKeyIndex);
   %Nukey = %Emitter.addDataKey(%this.getText(), %keyval);
   
   }
   else
   {
   
    %value = %this.getText();

   %asset.selectField(%this.Fieldname);
   
   %count = %asset.getDataKeyCount();
   
   if(%count==1)
   {
      echo("Base Key must stay at 0.0");
      return;
   }

   %key = %asset.getDataKey(%this.DataKeyIndex);
   %keyval =  %key._1;
   
   %asset.removeDataKey(%this.DataKeyIndex);
   %asset.addDataKey(%this.getText(), %keyval);
   }

ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}