function ImageSlicer::prepstackParam(%this)
{
//CellCountX
   %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };
      
   %txt = new GuiTextCtrl(){
      text = "Cell Count X";
      Profile=GuiInspectorTextEditCenterProfile;
      Extent = "90 20";      
      };
      %stk.add(%txt);
         
   %type = new GuiTextEditCtrl(){
      class = DataEntry_TB;
      Profile = GuiNumberEditProfile;
      Extent = "90 20";
      text = "1";
      valtomod = "CellCountX";
   };
   %stk.add(%type);
   DataEntry_Set.add(%type);
    StackSet.add(%stk);
      
   IMGSlicerStack.add(%stk);
   
//CellCountY
   %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };
      
   %txt = new GuiTextCtrl(){
      text = "Cell Count Y";
      Profile=GuiInspectorTextEditCenterProfile;
      Extent = "90 20";      
      };
      %stk.add(%txt);
   
   %type = new GuiTextEditCtrl(){
      class = DataEntry_TB;
      Profile = GuiNumberEditProfile;
      Extent = "90 20";
      text = "1";
      valtomod = "CellCountY";
   };
   %stk.add(%type);
   DataEntry_Set.add(%type);
    StackSet.add(%stk);
      
   IMGSlicerStack.add(%stk);
   
//CellWidth
   %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };
      
   %txt = new GuiTextCtrl(){
      text = "Cell Width";
      Profile=GuiInspectorTextEditCenterProfile;
      Extent = "90 20";      
      };
      %stk.add(%txt);
   
   %type = new GuiTextEditCtrl(){
      class = DataEntry_TB;
      Profile = GuiNumberEditProfile;
      Extent = "90 20";
      text = "1";
      valtomod = "CellWidth";
   };
   %stk.add(%type);
   DataEntry_Set.add(%type);
    StackSet.add(%stk);
      
   IMGSlicerStack.add(%stk);
   
//CellHeight
   %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };
      
   %txt = new GuiTextCtrl(){
      text = "CellHeight";
      Profile=GuiInspectorTextEditCenterProfile;
      Extent = "90 20";
      };
      %stk.add(%txt);
   
   %type = new GuiTextEditCtrl(){
      class = DataEntry_TB;
      Profile = GuiNumberEditProfile;
      Extent = "90 20";
      text = "1";
      valtomod = "CellHeight";
   };
   %stk.add(%type);
   DataEntry_Set.add(%type);
    StackSet.add(%stk);
      
   IMGSlicerStack.add(%stk);

//CellStrideX
   %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };
      
   %txt = new GuiTextCtrl(){
      text = "CellStrideX";
      Profile=GuiInspectorTextEditCenterProfile;
      Extent = "90 20";      
      };
      %stk.add(%txt);
   
   %type = new GuiTextEditCtrl(){
      class = DataEntry_TB;
      Profile = GuiNumberEditProfile;
      Extent = "90 20";
      valtomod = "CellStrideX";
   };
   %stk.add(%type);
   DataEntry_Set.add(%type);
    StackSet.add(%stk);
      
   IMGSlicerStack.add(%stk);

//CellStrideY
   %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };
      
   %txt = new GuiTextCtrl(){
      text = "CellStrideY";
      Profile=GuiInspectorTextEditCenterProfile;
      Extent = "90 20";      
      };
      %stk.add(%txt);
   
   %type = new GuiTextEditCtrl(){
      class = DataEntry_TB;
      Profile = GuiNumberEditProfile;
      Extent = "90 20";
      valtomod = "CellStrideY";
   };
   %stk.add(%type);
   DataEntry_Set.add(%type);
    StackSet.add(%stk);
      
   IMGSlicerStack.add(%stk);
   
//CellOffsetX
   %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };
      
   %txt = new GuiTextCtrl(){
      text = "CellOffsetX";
      Profile=GuiInspectorTextEditCenterProfile;
      Extent = "90 20";      
      };
      %stk.add(%txt);
   
   %type = new GuiTextEditCtrl(){
      class = DataEntry_TB;
      Profile = GuiNumberEditProfile;
      Extent = "90 20";
      valtomod = "CellOffsetX";
   };
   %stk.add(%type);
   DataEntry_Set.add(%type);
    StackSet.add(%stk);
      
   IMGSlicerStack.add(%stk);
   
//CellOffsetY
   %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };
      
   %txt = new GuiTextCtrl(){
      text = "CellOffsetY";
      Profile=GuiInspectorTextEditCenterProfile;
      Extent = "90 20";      
      };
      %stk.add(%txt);
   
   %type = new GuiTextEditCtrl(){
      class = DataEntry_TB;
      Profile = GuiNumberEditProfile;
      Extent = "90 20";
      valtomod = "CellOffsetY";
   };
   %stk.add(%type);
   DataEntry_Set.add(%type);
    StackSet.add(%stk);
      
   IMGSlicerStack.add(%stk);

   IMGSlicerStack.updateStack();
}

function ImageSlicer::setupAssetStatsUI(%this)
{
AssetWidthGUI.setText($CURRENTASSET.getImageWidth());
AssetHeightGUI.setText($CURRENTASSET.getImageHeight());
}

function ImageSlicer::prepstackExplicit(%this)
{
   //We need a list of Frames, buttons to add/remove frames

   //0 means we create
   //1 means we edit   
   $EXPLICITMODE = 0;
   $CURRENTEXPFRAME = 0;
   
      new ScriptObject(EXPDATA_OBJ){
         FrameCount = 0;
      };

    %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };
      
   %txt = new GuiTextCtrl(){
      text = "Lower Bounds";
      Profile=GuiInspectorTextEditCenterProfile;
      Extent = "70 20";      
      };
      %stk.add(%txt);
         
   %type = new GuiTextEditCtrl(){
      class = DataEntryEXP_TB;
      Profile = GuiNumberEditProfile;
      Extent = "60 20";
      text = "0";
      valtomod = "X";
   };
   %typeb = new GuiTextEditCtrl(){
      class = DataEntryEXP_TB;
      Profile = GuiNumberEditProfile;
      Extent = "60 20";
      text = "0";
      valtomod = "Y";
   };
   %stk.add(%type);
   DataEntry_Set.add(%type);
   %stk.add(%typeb);
   DataEntry_Set.add(%typeb);
    StackSet.add(%stk);
    
   IMGSlicerStack.add(%stk);
   
    %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };
      
   %txt = new GuiTextCtrl(){
      text = "Size";
      Profile=GuiInspectorTextEditCenterProfile;
      Extent = "70 20";      
      };
      %stk.add(%txt);
         
   %type = new GuiTextEditCtrl(){
      class = DataEntryEXP_TB;
      Profile = GuiNumberEditProfile;
      Extent = "60 20";
      text = "0";
      valtomod = "X1";
   };
   %typeb = new GuiTextEditCtrl(){
      class = DataEntryEXP_TB;
      Profile = GuiNumberEditProfile;
      Extent = "60 20";
      text = "0";
      valtomod = "Y1";
   };
   %stk.add(%type);
   DataEntry_Set.add(%type);
   %stk.add(%typeb);
   DataEntry_Set.add(%typeb);
   StackSet.add(%stk);
      
   IMGSlicerStack.add(%stk);
   
   %list = new GuiListBoxCtrl(ExplicitList){
      Profile = GuiListBoxProfile;
      class = ExplicitListCtrl;
      AllowMultipleSelections = false;
   };
   IMGSlicerStack.add(%list);
   
   StackSet.add(%list);
   
   %stk = new GuiStackControl(){
      StackingType = "Horizontal";
      HorizStacking = "Left to Right";
      Extent = "224 30";
      };

      %subframebtn = new GuiButtonCtrl(){
      Profile = BlueButtonProfile;
      Extent = "35 15";
      Text = "REM";
      Command = "ImageSlicer.remexplicitframe();";
   };
   %stk.add(%subframebtn);
   StackSet.add(%stk);
   
   %clp = new GuiColorPickerCtrl(){
      Extent = "224 224";
      class = CPick_class;
      ShowSelector = true;
      DisplayMode = "HorizColor";
   };
   StackSet.add(%clp);
   IMGSlicerStack.add(%clp);
   
   IMGSlicerStack.add(%stk);
   IMGSlicerStack.updateStack();
}