function ParticleEditor::AddDataKeyCategory(%this, %CatName)
{
   if(!isObject(MainCategorySet)) new SimSet(MainCategorySet);
   
   %Catrollout = new GuiRolloutCtrl()
   {
      Caption = %CatName;
      ClickCollapse = true;
      Profile = "GuiInspectorGroupProfile";
      Position = "0 0";
      Extent = "235 0";
      HorizSizing = "right";
      };
   UIFlush.add(%Catrollout);
   
   MainCategorySet.add(%Catrollout);

      %CatUI = new GuiStackControl(){
      StackingType = "Vertical";
      VertStacking = "Top to Bottom";
      HorizSizing = "center";
      Profile = "GuiSunkenContainerProfile";
      VertSizing = "center";
      };      
      UIFlush.add(%CatUI);
      %Catrollout.add(%CatUI);
   
   return(%CatUI);
}

function ParticleEditor::BuildDataKeyUI(%this, %stack, %asset, %EmitterIndex, %FieldName, %type)
{
   %Emitter = %asset.getEmitter(%EmitterIndex);   

     %MainFieldRollout = new GuiRolloutCtrl()
      {
      Caption = %FieldName;
      ClickCollapse = true;
      Profile = "GuiInspectorGroupProfile";
      Position = "0 0";
      Extent = "235 0";
      HorizSizing = "right";
      };
      UIFlush.add(%MainFieldRollout);

      %GroupEmiStack = new GuiStackControl(){
      StackingType = "Vertical";
      VertStacking = "Top to Bottom";
      HorizSizing = "relative";
      Profile = "GuiSunkenContainerProfile";
      };      
      UIFlush.add(%GroupEmiStack);
      %MainFieldRollout.add(%GroupEmiStack);
            
      %stack.add(%MainFieldRollout);
      
      for(%typeitr = 0; %typeitr < %type+1; %typeitr++)
      {
switch$(%typeitr){
   case 0: %NewFieldName = %FieldName;
           %FriendlyName = %FieldName@"Base";
   case 1: %NewFieldName = %FieldName@"Variation";
           %FriendlyName = %FieldName@"Variation";
   case 2: %NewFieldName = %FieldName@"Life";
           %FriendlyName = %FieldName@"Life";
               }
      
   %Emitter.selectField(%NewFieldName);
   %count = %Emitter.getDataKeyCount();
      
         %label = new GuiTextCtrl(){
            Position = "40 0";
            Profile = "GuiPFXHeaderProfile";
            Extent = "235 20";
            text = %FriendlyName;
            HorizSizing = "center";
            VertSizing = "center";
            };
            UIFlush.add(%label);
         %GroupEmiStack.add(%label);
      
       %toptab = new GuiTabBookCtrl(){
            TabHeight = 20;
            Profile = "GuiTabProfile";
            Extent = "235 280";
         };         
         UIFlush.add(%toptab);
         %GroupEmiStack.add(%toptab);
         
  for(%keytr = 0; %keytr < %count; %keytr++)
         {
            %this.AddDataKeyTab( %keytr, %toptab, %Emitter, %EmitterIndex, %NewFieldName, %count, %typeitr);
         }
      }
%MainFieldRollout.add(%GroupEmiStack);
}
 
function ParticleEditor::AddDataKeyTab(%this, %keytr, %toptab, %Emitter, %EmitterIndex, %FieldName, %count, %type)
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
         
         %Key = %Emitter.getDataKey(%keytr);
         %K_time = %Key._0;
         %K_value = %Key._1;
         
         %timeValue = new GuiTextEditCtrl(){
              class = KeyTimeTXTED;
              Extent = "64 32";
              tabComplete = true;
              VertSizing = "relative";
              DataKeyIndex = %keytr;
              FieldName = %FieldName;
              isEmitter = true;
              Text = mFloatLength(%K_time,2);
              EmitterIndex = %EmitterIndex;
              TimeLineType = %type;
              };
            UIFlush.add(%timeValue);
         
           %textEdit = new GuiSliderCtrl(){
            class = EmitterSlider;
            Profile = "GuiSliderProfile";
            Value = mFloatLength(%K_value,2);
            Position = "130 0";            
            Extent = "160 0";
            range = %Emitter.getMinValue() SPC %Emitter.getMaxValue();
            ticks = 10;
            HorizSizing = "relative";
            VertSizing = "top";
            //dynamic fields
            Fieldname = %FieldName;
            EmitterIndex = %EmitterIndex;
            _KeyIndex = %keytr;
            };
         UIFlush.add(%textEdit);
         
          %textEdit.ValCtrl = %this.MinMaxSetup(%tabstack, %Emitter, %textEdit, %FieldName, %keytr, 1);
 
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
          EmitterIndex = %EmitterIndex;
          tooltipprofile = "GuiToolTipProfile";
          ToolTip = "Remove this DataKey";
          tooltipWidth = "250";
          IsEmitter = true;
          DataKeyIndex = %keytr;
          FieldName = %FieldName;
          TabTop = %toptab;
          TabCur = %tab;
          TimeLineType = %type;
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
          EmitterIndex = %EmitterIndex;
          FieldName = %FieldName;
          DataKeyIndex = %keytr;
          IsEmitter = true;
          TabTop = %toptab;
          TimeLineType = %type;
       };
       UIFlush.add(%AddEmitter);
       %adddelstack.add(%AddEmitter);

         %tabstack.add(%adddelstack);

         %tab.add(%tabstack);
         %toptab.add(%tab);
}