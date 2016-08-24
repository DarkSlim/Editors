function ParticleEditor::populateInspector(%this, %Effect, %AssetName)
{
   if(isObject(%Effect))
   {
      %asset = AssetDatabase.acquireAsset(%Effect.Particle);     
       
      PFX_EffectRollout.Caption = "Asset Keys";
      
      %stack = new GuiStackControl(){
         StackingType = "Vertical";
         VertStacking = "Top to Bottom";
         HorizSizing = "center";
         VertSizing = "center";
         Profile = "GuiSunkenContainerProfile";
      };
      UIFlush.add(%stack);
      
      %NameEdit = new GuiTextEditCtrl(AssetNameEditCTRL)
      {
         text = %AssetName;         
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiTextEditProfile;
         tabComplete = true;
      };
      UIFlush.add(%NameEdit);
      %stack.add(%NameEdit);
            
      AssetNameEditCTRL.setText($PFXASSETNAME);
      
      for(%itr = 0; %itr < %asset.getSelectableFieldCount(); %itr++)
      {
         %Fieldname = %asset.getSelectableFieldName(%itr);
         %asset.selectField(%Fieldname);
         
            %label = new GuiTextCtrl(){
            Position = "40 0";
            Profile = "GuiPFXHeaderProfile";
            Extent = "100 20";
            text = %Fieldname;
            HorizSizing = "center";
            VertSizing = "center";
            };
            UIFlush.add(%label);
         %stack.add(%label);
                     
         %toptab = new GuiTabBookCtrl(){
            TabHeight = 20;
            Profile = "GuiTabProfile";
            Extent = "235 200";
         };
         UIFlush.add(%toptab);
         
      for(%keytr = 0; %keytr < %asset.getDataKeyCount(); %keytr++)
         {
         %tab = new GuiTabPageCtrl(){
         text = %keytr;
         Profile = "GuiTabProfile";
         isContainer = true;
         };
        UIFlush.add(%tab);
        
         %tabstack = new GuiStackControl(){
         StackingType = "Vertical";
         VertStacking = "Top to Bottom";
         Position = "0 32";
         MinExtent = "235 0";
         HorizSizing = "center";         
         VertSizing = "top";
         Profile = "GuiSunkenContainerProfile";
         };
         UIFlush.add(%tabstack);
         
         %timestack = new GuiStackControl(){
         StackingType = "Horizontal";
         VertStacking = "Left to Right";
         MinExtent = "235 32";
         HorizSizing = "center";         
         VertSizing = "top";
         Profile = "GuiSunkenContainerProfile";
         };
         UIFlush.add(%timestack);
         
         %Key = %asset.getDataKey(%keytr);
         %K_time = %Key._0;
         %K_value = %Key._1;
         
           %timeValue = new GuiTextEditCtrl(){
              class = KeyTimeTXTED;
              Extent = "64 32";
              tabComplete = true;
              VertSizing = "top";
              DataKeyIndex = %keytr;
              FieldName = %Fieldname;
              Text = %K_time;
              isEmitter = false;
              };
            UIFlush.add(%timeValue);
         
           %textEdit = new GuiSliderCtrl(){
            class = EffectSlider;
            Value = mFloatLength(%K_value,2);
            Position = "130 0";            
            Extent = "160 20";
             range = %asset.getMinValue() SPC %asset.getMaxValue();
            ticks = 10;
            HorizSizing = "relative";
            VertSizing = "bottom";
            //dynamic fields
            Fieldname = %Fieldname;
            Particle_Editor = %this;
            _KeyIndex = %keytr;
            };
         UIFlush.add(%textEdit);

         %textEdit.ValCtrl = %this.MinMaxSetup(%tabstack, %asset, %textEdit, %Fieldname, %keytr, 0);
         
         %timestack.add(%timeValue);
         %timestack.add(%textEdit);
         
         %tabstack.add(%timestack);
         
         %TabButtonsstack = new GuiStackControl(){
         StackingType = "Horizontal";
         VertStacking = "Left To Right";
         HorizSizing = "left";
         VertSizing = "center";
         Extent = "235 32";
         Profile = "GuiSunkenContainerProfile";
         };
         UIFlush.add(%TabButtonsstack);         
         %tabstack.add(%TabButtonsstack);

         %count = %asset.getDataKeyCount();         

          %MinusEmitter = new GuiBitmapButtonCtrl(){
          class = RemDKBTN;
          bitmap = "../gui/images/minusButton";
          Extent = "36 16";
          tooltipprofile = "GuiToolTipProfile";
          ToolTip = "Remove this DataKey";
          tooltipWidth = "250";
          IsEmitter = false;
          DataKeyIndex = %keytr;
          FieldName = %FieldName;
          TabTop = %toptab;
         };
         UIFlush.add(%MinusEmitter);
         %TabButtonsstack.add(%MinusEmitter);
         
         %AddEmitter = new GuiBitmapButtonCtrl(){
          class = AddDKBTN;
          bitmap = "../gui/images/plusButton";
          Extent = "36 16";
          tooltipprofile = "GuiToolTipProfile";
          ToolTip = "Add new Data Key";
          tooltipWidth = "250";
          FieldName = %FieldName;
          DataKeyIndex = %keytr;
          IsEmitter = false;
          TabTop = %toptab;
       };
         UIFlush.add(%AddEmitter);
         %TabButtonsstack.add(%AddEmitter);

  
         %tab.add(%tabstack);
         %toptab.add(%tab);
         }
         %stack.add(%toptab);
         }
       
PFX_EffectRollout.add(%stack);
      }
   %this.populateEffectStatic(%Effect);
}

function ParticleEditor::populateEffectStatic(%this, %Effect)
{
%asset = AssetDatabase.acquireAsset(%Effect.Particle);

%label = new GuiTextCtrl(){
   text = "Life Mode";
   HorizSizing = "right";
   Extent = "100 15";
   VertSizing = "center";
};
UIFlush.add(%label);
ValueStack.add(%label);

%lifemodepopup = new GuiPopUpMenuCtrl(LifeModeCtrl){
      Profile = "GuiPopUpMenuProfile";
      HorizSizing = "center";
      VertSizing = "center";
      Position = "0 0";
      Extent = "180 25";
      canSave = 0;
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Selects the Effect's LifeMode";
      tooltipWidth = "250";
      MaxLength = "1024";
      maxPopupHeight = "200";
      bitmapBounds = "16 16";
};
UIFlush.add(%lifemodepopup);
ValueStack.add(%lifemodepopup);

%lifemode = %asset.getLifeMode();

%lifemodepopup.add("KILL",0);
%lifemodepopup.add("CYCLE",1);
%lifemodepopup.add("STOP",2);
%lifemodepopup.add("INFINITE",3);

%lifemodepopup.setText(%lifemode);

%label = new GuiTextCtrl(){
   text = "LifeTime" SPC %asset.getLifetime();
   HorizSizing = "right";
   Extent = "64 15";
   VertSizing = "center";
};
UIFlush.add(%label);
ValueStack.add(%label);

  %lifeTimeslider = new GuiSliderCtrl(){
            class = LifeTimeSlider;
            Value = %asset.getLifetime();
            Position = "130 0";
            Extent = "160 20";
            range = "0 25";
            ticks = 50;
            HorizSizing = "relative";
            VertSizing = "bottom";
            //dynamic fields
            Link = %label;
            };
         UIFlush.add(%lifeTimeslider);
         ValueStack.add(%lifeTimeslider);
}


function ParticleEditor::AddAssetDataKeyTab(%this, %keytr, %toptab, %asset, %FieldName, %count)
{
         %tab = new GuiTabPageCtrl(){
         text = %keytr;
         Profile = "GuiTabProfile";
         isContainer = true;
         };
        UIFlush.add(%tab);
        
         %tabstack = new GuiStackControl(){
         StackingType = "Vertical";
         VertStacking = "Top to Bottom";
         //Position = "0 32";
         //HorizSizing = "left";
         //VertSizing = "top";
         MinExtent = "235 0";
         Profile = "GuiSunkenContainerProfile";
         };
         UIFlush.add(%tabstack);
         
         %timestack = new GuiStackControl(){
         StackingType = "Horizontal";
         VertStacking = "Left to Right";         
         HorizSizing = "left";         
         VertSizing = "top";
         Profile = "GuiSunkenContainerProfile";
         };
         UIFlush.add(%timestack);
         
         %Key = %asset.getDataKey(%keytr);
         %K_time = %Key._0;
         %K_value = %Key._1;
         
         %timeValue = new GuiTextEditCtrl(){
              class = KeyTimeTXTED;
              Extent = "64 32";
              tabComplete = true;
              VertSizing = "relative";
              DataKeyIndex = %keytr;
              FieldName = %FieldName;
              isEmitter = false;
              Text = mFloatLength(%K_time,2);
              };
            UIFlush.add(%timeValue);
         
           %textEdit = new GuiSliderCtrl(){
            class = EffectSlider;
            Profile = "GuiSliderProfile";
            Value = mFloatLength(%K_value,2);
            Position = "130 0";            
            Extent = "160 0";
            range = %asset.getMinValue() SPC %asset.getMaxValue();
            ticks = 10;
            HorizSizing = "relative";
            VertSizing = "top";
            //dynamic fields
            Fieldname = %FieldName;
            _KeyIndex = %keytr;
            };
         UIFlush.add(%textEdit);
         
          %textEdit.ValCtrl = %this.MinMaxSetup(%tabstack, %asset, %textEdit, %FieldName, %keytr, 0);
 
            %timestack.add(%textEdit);
            %timestack.add(%timeValue);
            %tabstack.add(%timestack);
            
         %adddelstack = new GuiStackControl(){
         StackingType = "Horizontal";
         VertStacking = "Left to Right";         
         HorizSizing = "left";         
         VertSizing = "top";
         Extent = "235 32";
         Profile = "GuiSunkenContainerProfile";
         };
         UIFlush.add(%adddelstack);
         
           %MinusEmitter = new GuiBitmapButtonCtrl(){
          class = RemDKBTN;
          bitmap = "../gui/images/minusButton";
          Extent = "36 16";
          tooltipprofile = "GuiToolTipProfile";
          ToolTip = "Remove this DataKey";
          tooltipWidth = "250";
          IsEmitter = false;
          DataKeyIndex = %keytr;
          FieldName = %FieldName;
          TabTop = %toptab;
          TabCur = %tab;
       };
       UIFlush.add(%MinusEmitter);
       %adddelstack.add(%MinusEmitter);
       
          %AddEmitter = new GuiBitmapButtonCtrl(){
          class = AddDKBTN;
          bitmap = "../gui/images/plusButton";
          Extent = "36 16";
          tooltipprofile = "GuiToolTipProfile";
          ToolTip = "Add new Data Key";
          tooltipWidth = "250";
          FieldName = %FieldName;
          DataKeyIndex = %keytr;
          IsEmitter = false;
          TabTop = %toptab;
       };
       UIFlush.add(%AddEmitter);
       %adddelstack.add(%AddEmitter);
     
         %tabstack.add(%adddelstack);

         %tab.add(%tabstack);
         %toptab.add(%tab);
}    