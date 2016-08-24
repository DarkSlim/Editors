function ParticleEditor::PlayEffect(%this)
{
%this.PFX.play();
}

function ParticleEditor::PauseEffect(%this)
{
if(%this.PFX.getPaused())
%this.PFX.setPaused(false);
else
%this.PFX.setPaused(true);
}

function ParticleEditor::resetPFXEDITOR_KeepAsset(%this, %asset)
{
$SuicideVictim = false;

if(isObject(UIFlush))
{
   UIFlush.deleteObjects();
   echo("UIFlush counts" SPC UIFlush.getCount() SPC "members");
}
else
   new SimSet(UIFlush);

Canvas.pushDialog(PFX_Preview);

PFX_SW.setCameraSize(10,10);
PFX_SW.setCameraZoom(1.0);

if(isObject(PFX_InputListener))
PFX_InputListener.delete();

new ScriptObject(PFX_InputListener);
PFX_SW.addInputListener(PFX_InputListener);

if(isObject(%this.PFXscene))
{
   PFX_SW.resetScene();
   %this.PFXScene.delete();
}

%Scene = new Scene();

PFX_SW.setScene(%Scene);

%this.PFXscene = %Scene;

 %NewEff = new ParticlePlayer()
   {
      Particle = %asset.getAssetId();
   };
%this.isPaused = false;
%this.PFX = %NewEff;

%Scene.add(%NewEff);

PFX_SW.setCameraPosition(0,0);
PFX_SW.setCameraZoom(1.0);

%this.populateInspector(%NewEff, %AssetName);

   %E_Count = %asset.getEmitterCount();
   
   for(%itr = 0; %itr < %E_Count; %itr++)
   {
   %Emitter = %asset.getEmitter(%itr);
	%Collapse = %this.populateEmittersInspector(%NewEff, %itr);
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


function ParticleEditor::resetPFXEDITOR(%this)
{
$SuicideVictim = false;

if(isObject(UIFlush))
{
   UIFlush.deleteObjects();
   echo("UIFlush counts" SPC UIFlush.getCount() SPC "members");
}
else
   new SimSet(UIFlush);

Canvas.pushDialog(PFX_Preview);

PFX_SW.setCameraSize(10,10);
PFX_SW.setCameraZoom(1.0);

if(isObject(PFX_InputListener))
PFX_InputListener.delete();

new ScriptObject(PFX_InputListener);
PFX_SW.addInputListener(PFX_InputListener);

if(isObject(%this.PFXscene))
{
   PFX_SW.resetScene();
   %this.PFXScene.delete();
}

%Scene = new Scene();

PFX_SW.setScene(%Scene);

%this.PFXscene = %Scene;

PFX_SW.setCameraPosition(0,0);
PFX_SW.setCameraZoom(1.0);
}

function ParticleEditor::StopEffect(%this, %val)
{
 %this.PFX.stop(true, false);
}

function ParticleEditor::SetLifeMode(%this, %mode)
{
%asset = AssetDatabase.acquireAsset(%this.PFX.Particle);

if(%mode $= "KILL")
{
$SuicideVictim = true;
%mode = "STOP";
}

%asset.setLifeMode(%mode);
%this.PFX.play();
}

function ParticleEditor::populateEmittersInspector(%this, %Effect, %EmitterIndex)
{
   %asset = AssetDatabase.acquireAsset(%Effect.Particle);
   %Emitter = %asset.getEmitter(%EmitterIndex);   

      %Rollout = new GuiRolloutCtrl()
      {
      Caption = "Emitter" SPC %EmitterIndex;
      ClickCollapse = true;
      Profile = "GuiInspectorGroupProfile";
      Position = "0 0";
      HorizSizing = "right";
      };
      UIFlush.add(%Rollout);

      %stack = new GuiStackControl(){
      StackingType = "Vertical";
      VertStacking = "Top to Bottom";
      HorizSizing = "relative";
      Profile = "GuiSunkenContainerProfile";
      };      
      UIFlush.add(%stack);
      %Rollout.add(%stack);
      
      %NameEdit = new GuiTextEditCtrl()
      {
         class = NameEditCTRL;
         text = %Emitter.getEmitterName();         
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiTextEditProfile;
         tabComplete = true;
               
               //dynamic
         EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%NameEdit);
      %stack.add(%NameEdit);
      
      %EmitterStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
      };
      %stack.add(%EmitterStack);

      %ImageAssetID = %Emitter.getImage();

      if(%ImageAssetID $= "")
      {
         %Animation = %Emitter.getAnimation();
         
         if(%Animation $= "")
         {
          %ImageAssetID = "ToyAssets:hexagon";
         }
         else
         {
         %isAnim = true;
         %anim_asset = AssetDatabase.acquireAsset(%Animation);
         %ImageAssetID = %anim_asset.getImage();
         }
      }      
      
      %ImageButton = new GuiButtonCtrl()
      {
         class = ImageChooser;
         Profile = "BlueButtonProfile";
         Extent = "64 64";
         HorizSizing = "right";
         Position = "0 0";
         //Command = "ParticleEditor.ShowImageCh();";
         //dynamic fields
         EmitterIndex = %EmitterIndex;
         IMG =  %ImageAssetID;
         Anim = %Animation;
         IsAnim = %isAnim;
      };
        %EmitterStack.add(%ImageButton);
         
       %play = new GuiBitmapButtonCtrl(){
          class = EmitterVisibleBTN;
          bitmap = "../gui/images/EyesOpen";
          Extent = "56 32";
          EmitterIndex = %EmitterIndex;
          buttonType = "ToggleButton";
          tooltipprofile = "GuiToolTipProfile";
          ToolTip = "Toggles Emitter Visibility";
          tooltipWidth = "250";
       };
       %EmitterStack.add(%play);
       
       %pause = new GuiBitmapButtonCtrl(){
          class = EmitterPauseBTN;
          bitmap = "../gui/images/StopButton";
          Extent = "56 32";
          EmitterIndex = %EmitterIndex;
          tooltipprofile = "GuiToolTipProfile";
          ToolTip = "Starts / Stops Emitter";
          tooltipWidth = "250";
       };
       %EmitterStack.add(%pause);
       
      %RandomFrameCTRL = new GuiCheckBoxCtrl(){
           class = RandomFrameCtrl;
           tooltipprofile = "GuiToolTipProfile";
            ToolTip = "Use Random Image Frame";
            tooltipWidth = "250";
            text = "";
            EmitterIndex = %EmitterIndex;
            };
            UIFlush.add(%RandomFrameCTRL);
            %EmitterStack.add(%RandomFrameCTRL);
      
         if(%Emitter.getRandomImageFrame())
            %RandomFrameCTRL.setStateOn(true);
         else
            %RandomFrameCTRL.setStateOn(false);
   
      %EmitDup = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%EmitDup);
      
         %MinusEmitter = new GuiBitmapButtonCtrl(){
          class = RemEmiBTN;
          bitmap = "../gui/images/minusButton";
          Extent = "36 32";
          EmitterIndex = %EmitterIndex;
          tooltipprofile = "GuiToolTipProfile";
          ToolTip = "Remove this Emitter";
          tooltipWidth = "250";
          Rollout = %Rollout;
       };
       %EmitDup.add(%MinusEmitter);
       
       %AddEmitter = new GuiBitmapButtonCtrl(){
          class = AddEmiBTN;
          bitmap = "../gui/images/plusButton";
          Extent = "36 32";
          EmitterIndex = %EmitterIndex;
          tooltipprofile = "GuiToolTipProfile";
          ToolTip = "Add new Emitter";
          tooltipWidth = "250";
       };
       %EmitDup.add(%AddEmitter);

%typeStack = new GuiStackControl()
      {
         StackingType = "Vertical";
         HorizStacking = "Top to Bottom";
         Extent = "235 64";
      };
      %stack.add(%typeStack);

%label = new GuiTextCtrl(){
   text = "Emitter Mode";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "64 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%typeStack.add(%label);

%EmitterTypePopup = new GuiPopUpMenuCtrl(){
      class = EmiTypeCtrl;
      Profile = "GuiPopUpMenuProfile";
      HorizSizing = "center";
      VertSizing = "center";
      Position = "0 0";
      Extent = "180 25";
      canSave = 0;
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Selects the Emitter's Type";      
      tooltipWidth = "250";
      MaxLength = "1024";      
      maxPopupHeight = "200";
      EmitterIndex = %EmitterIndex;
      bitmapBounds = "16 16";
};
UIFlush.add(%EmitterTypePopup);
%typeStack.add(%EmitterTypePopup);

%EmiType = %Emitter.getEmitterType();


%EmitterTypePopup.add("POINT",0);
%EmitterTypePopup.add("LINE",1);
%EmitterTypePopup.add("BOX",2);
%EmitterTypePopup.add("DISK",3);
%EmitterTypePopup.add("ELLIPSE",4);
%EmitterTypePopup.add("TORUS",5);

%EmitterTypePopup.setText(%EmiType);

//------------------------------------------
%OrStack = new GuiStackControl()
      {
         StackingType = "Vertical";
         HorizStacking = "Top to Bottom";
         Extent = "235 64";
      };
      %stack.add(%OrStack);

%label = new GuiTextCtrl(){
   text = "Orientation";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "64 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%OrStack.add(%label);

%OrientPopup = new GuiPopUpMenuCtrl(){
      class = OriTypeCtrl;
      Profile = "GuiPopUpMenuProfile";
      HorizSizing = "center";
      VertSizing = "center";
      Position = "0 0";
      Extent = "180 25";
      canSave = 0;
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Selects the Emitter's Orientation";
      tooltipWidth = "250";
      MaxLength = "1024";      
      maxPopupHeight = "200";
      EmitterIndex = %EmitterIndex;
      bitmapBounds = "16 16";
};
UIFlush.add(%OrientPopup);
%OrStack.add(%OrientPopup);

%OriType = %Emitter.getOrientationType();

%OrientPopup.add("FIXED",0);
%OrientPopup.add("ALIGNED",0);
%OrientPopup.add("RANDOM",0);

%OrientPopup.setText(%OriType);

//------------------------------------------

%label = new GuiTextCtrl(){
   text = "Emitter Size";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "64 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%stack.add(%label);

%SizeStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%SizeStack);
      
      %size = %Emitter.getEmitterSize();
      
      %WidthCtrl = new GuiTextEditCtrl()
      {
         class = EmiSizeCTRL;
         text = %size._0;
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;                        
               //dynamic
         EmitterIndex = %EmitterIndex;
         SizeType = 0;
      };
      UIFlush.add(%WidthCtrl);
      %SizeStack.add(%WidthCtrl);
      
      %HeightCtrl= new GuiTextEditCtrl()
      {
         class = EmiSizeCTRL;
         text = %size._1;
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;                        
               //dynamic
         EmitterIndex = %EmitterIndex;
         SizeType = 1;
      };
      UIFlush.add(%HeightCtrl);
      %SizeStack.add(%HeightCtrl);
      

%label = new GuiTextCtrl(){
   text = "   Emitter Offset";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "120 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%stack.add(%label);
      
%OffsetStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%OffsetStack);
      
      %OffsetXCtrl = new GuiTextEditCtrl()
      {
         class = OffSizeCTRL;
         text = %size._0;
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;                        
               //dynamic
         EmitterIndex = %EmitterIndex;
         SizeType = 0;
      };
      UIFlush.add(%OffsetXCtrl);
      %OffsetStack.add(%OffsetXCtrl);
      
      %OffsetYCtrl= new GuiTextEditCtrl()
      {
         class = OffSizeCTRL;
         text = %size._1;
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;                        
               //dynamic
         EmitterIndex = %EmitterIndex;
         SizeType = 1;
      };
      UIFlush.add(%OffsetYCtrl);
      %OffsetStack.add(%OffsetYCtrl);
       //Add Remove Emitters
       
      %AttachCtrl = new GuiCheckBoxCtrl()
      {
      class = AttachPosCtrl;
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Attach Position to Emitter";
      tooltipWidth = "250";
      text = "";
      EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%AttachCtrl);
      %OffsetStack.add(%AttachCtrl);
      
      if(%Emitter.getAttachPositionToEmitter())
         %AttachCtrl.setStateOn(true);
      else
         %AttachCtrl.setStateOn(false);
                  


%label = new GuiTextCtrl(){
   text = "Pivot Point";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "64 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%stack.add(%label);

%PivotStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%PivotStack);
      
      %pivot = %Emitter.getPivotPoint();
      
      %WidthCtrl = new GuiTextEditCtrl()
      {
         class = PivotCTRL;
         text = %pivot._0;
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;                        
               //dynamic
         EmitterIndex = %EmitterIndex;
         PivotType = 0;
      };
      UIFlush.add(%WidthCtrl);
      %PivotStack.add(%WidthCtrl);
      
      %HeightCtrl= new GuiTextEditCtrl()
      {
         class = PivotCTRL;
         text = %pivot._1;
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;                        
               //dynamic
         EmitterIndex = %EmitterIndex;
         PivotType = 1;
      };
      UIFlush.add(%HeightCtrl);
      %PivotStack.add(%HeightCtrl);
      
//------------------------------------------------


%EmitAnglestack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%EmitAnglestack);
      
   %label = new GuiTextCtrl(){
   text = "   Emitter Angle  ";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "120 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%EmitAnglestack.add(%label);

      %EmitAngleControl= new GuiTextEditCtrl()
      {
         class = EmitAngleCTRL;
         text = %Emitter.getEmitterAngle();
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;
         tooltipprofile = "GuiToolTipProfile";
         ToolTip = "Emitter Angle relative to the Effect";
         tooltipWidth = "250";
         //dynamic
         EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%EmitAngleControl);
      %EmitAnglestack.add(%EmitAngleControl);
//-------------------------------------------------------
%FixedForceAngleStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%FixedForceAngleStack);
      
   %label = new GuiTextCtrl(){
   text = "   FixedForce Angle  ";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "120 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%FixedForceAngleStack.add(%label);

  
      %FFAControl= new GuiTextEditCtrl()
      {
         class = FixedForceAngleCTRL;
         text = %Emitter.getFixedForceAngle();
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;
         tooltipprofile = "GuiToolTipProfile";
         ToolTip = "Fixed Force Angle";
         tooltipWidth = "250";
         //dynamic
         EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%FFAControl);
      %FixedForceAngleStack.add(%FFAControl);

//-------------------------------------------------------

   %BlendModeStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%BlendModeStack);
      
   %label = new GuiTextCtrl(){
   text = " Blend Mode ";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "100 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%BlendModeStack.add(%label);
      
      %BlendCTRL = new GuiCheckBoxCtrl()
      {
      class = BlendModeCtrl;      
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Whether to use Blend Mode or not";
      tooltipWidth = "250";
      text = "";
      EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%BlendCTRL);
      %BlendModeStack.add(%BlendCTRL);

      if(%Emitter.getBlendMode())
         %BlendCTRL.setStateOn(true);
      else
         %BlendCTRL.setStateOn(false);


%NewBlendStack = new GuiStackControl()
      {
         StackingType = "Vertical";
         HorizStacking = "Top to Bottom";
         Extent = "235 64";
      };
      %stack.add(%NewBlendStack);

%label = new GuiTextCtrl(){
   text = "SRC";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "64 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%NewBlendStack.add(%label);

%SRCPopup = new GuiPopUpMenuCtrl(){
      class = BlendFactorCtrl;
      Profile = "GuiPopUpMenuProfile";
      HorizSizing = "center";
      VertSizing = "center";
      Position = "0 0";
      Extent = "180 25";
      canSave = 0;
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Source Blend Factor";
      tooltipWidth = "250";
      MaxLength = "1024";      
      maxPopupHeight = "200";
      EmitterIndex = %EmitterIndex;
      bitmapBounds = "16 16";
      _Type = "src";
};
UIFlush.add(%SRCPopup);
%NewBlendStack.add(%SRCPopup);

%SrcType = %Emitter.getSrcBlendFactor();

%SRCPopup.add("ZERO",0);
%SRCPopup.add("ONE",1);
%SRCPopup.add("DST_COLOR",2);
%SRCPopup.add("ONE_MINUS_DST_COLOR",3);
%SRCPopup.add("SRC_ALPHA",4);
%SRCPopup.add("ONE_MINUS_SRC_ALPHA",5);
%SRCPopup.add("DST_ALPHA",6);
%SRCPopup.add("ONE_MINUS_DST_ALPHA",7);
%SRCPopup.add("SRC_ALPHA_SATURATE",8);

%SRCPopup.setText(%SrcType);


%label = new GuiTextCtrl(){
   text = "DST";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "64 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%NewBlendStack.add(%label);

%DSTPopup = new GuiPopUpMenuCtrl(){
      class = BlendFactorCtrl;
      Profile = "GuiPopUpMenuProfile";
      HorizSizing = "center";
      VertSizing = "center";
      Position = "0 0";
      Extent = "180 25";
      canSave = 0;
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Destination Blend Factor";
      tooltipWidth = "250";
      MaxLength = "1024";      
      maxPopupHeight = "200";
      EmitterIndex = %EmitterIndex;
      bitmapBounds = "16 16";
      _Type = "dst";
};
UIFlush.add(%DSTPopup);
%NewBlendStack.add(%DSTPopup);

%SrcType = %Emitter.getDstBlendFactor();

%DSTPopup.add("ZERO",0);
%DSTPopup.add("ONE",1);
%DSTPopup.add("SRC_COLOR",2);
%DSTPopup.add("ONE_MINUS_SRC_COLOR",3);
%DSTPopup.add("SRC_ALPHA",4);
%DSTPopup.add("ONE_MINUS_SRC_ALPHA",5);
%DSTPopup.add("DST_ALPHA",6);
%DSTPopup.add("ONE_MINUS_DST_ALPHA",7);
%DSTPopup.add("SRC_ALPHA_SATURATE",8);

%DSTPopup.setText(%SrcType);


%label = new GuiTextCtrl(){
   text = "Alpha Test";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "64 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%NewBlendStack.add(%label);

  %AlphaTestCTRL= new GuiTextEditCtrl()
      {
         class = AlphaTestCTRL;
         text = %Emitter.getFixedForceAngle();
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;
         tooltipprofile = "GuiToolTipProfile";
         ToolTip = "Alpha Test";
         tooltipWidth = "250";
         //dynamic
         EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%AlphaTestCTRL);
      %NewBlendStack.add(%AlphaTestCTRL);

//-----------------------------------------------------------

   %FixeAspectStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%FixeAspectStack);
      
   %label = new GuiTextCtrl(){
   text = " Fixed Aspect  ";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "100 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%FixeAspectStack.add(%label);
      
      %FixedAspect = new GuiCheckBoxCtrl()
      {
      class = FixedAngleCtrl;      
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Whether Graph Fields SizeX and SizeY are linked";
      tooltipWidth = "250";
      text = "";
      EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%FixedAspect);
      %FixeAspectStack.add(%FixedAspect);
      
      if(%Emitter.getFixedAspect())
         %FixedAspect.setStateOn(true);
      else
         %FixedAspect.setStateOn(false);

//-------------------------------------------------------

   %IntenseStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%IntenseStack);
      
   %label = new GuiTextCtrl(){
   text = " Intense Particle  ";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "100 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%IntenseStack.add(%label);
      
      %Intense = new GuiCheckBoxCtrl()
      {
      class = IntensePFXCtrl;      
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Particle Bloom";
      tooltipWidth = "250";
      text ="";
      EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%Intense);
      %IntenseStack.add(%Intense);
      
      if(%Emitter.getIntenseParticles())
         %Intense.setStateOn(true);
      else
         %Intense.setStateOn(false);
         
//-------------------------------------------------------

   %SingleStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%SingleStack);
      
   %label = new GuiTextCtrl(){
   text = " Single Particle";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "100 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%SingleStack.add(%label);
      
      %Single = new GuiCheckBoxCtrl()
      {
      class = SinglePFXCtrl;      
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Single Particle Mode";
      tooltipWidth = "250";
      text ="";
      EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%Single);
      %SingleStack.add(%Single);
      
      if(%Emitter.getSingleParticle())
         %Single.setStateOn(true);
      else
         %Single.setStateOn(false);
         
//-------------------------------------------------------

   %OldestStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%OldestStack);
      
   %label = new GuiTextCtrl(){
   text = " Oldest In Front";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "100 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%OldestStack.add(%label);
      
      %oldest = new GuiCheckBoxCtrl()
      {
      class = OldestPFXCtrl;      
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Oldest Particle In Front";
      tooltipWidth = "250";
      text ="";
      EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%oldest);
      %OldestStack.add(%oldest);
      
      if(%Emitter.getOldestInFront())
         %oldest.setStateOn(true);
      else
         %oldest.setStateOn(false);
         
//-------------------------------------------------------

   %AttachPosStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%AttachPosStack);
      
   %label = new GuiTextCtrl(){
   text = " Attach Pos";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "100 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%AttachPosStack.add(%label);
      
      %AttPosCTRL = new GuiCheckBoxCtrl()
      {
      class = AttPosCTRL;      
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Attach Position to Emitter";
      tooltipWidth = "250";
      text ="";
      EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%AttPosCTRL);
      %AttachPosStack.add(%AttPosCTRL);
      
      if(%Emitter.getAttachPositionToEmitter())
         %AttPosCTRL.setStateOn(true);
      else
         %AttPosCTRL.setStateOn(false);
         
//-------------------------------------------------------
 %AttachRotStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%AttachRotStack);
      
   %label = new GuiTextCtrl(){
   text = " Attach Rot";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "100 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%AttachRotStack.add(%label);
      
      %AttRotCTRL = new GuiCheckBoxCtrl()
      {
      class = AttRotCTRL;      
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Attach Rotation to Emitter";
      tooltipWidth = "250";
      text ="";
      EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%AttRotCTRL);
      %AttachRotStack.add(%AttRotCTRL);
      
      if(%Emitter.getAttachRotationToEmitter())
         %AttRotCTRL.setStateOn(true);
      else
         %AttRotCTRL.setStateOn(false);
         
//-------------------------------------------------------

 %LinkedStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%LinkedStack);
      
   %label = new GuiTextCtrl(){
   text = " Link Emission";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "100 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%LinkedStack.add(%label);
      
      %LinkCTRL = new GuiCheckBoxCtrl()
      {
      class = LERCTRL;      
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Link Emission to Rotation";
      tooltipWidth = "250";
      text ="";
      EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%LinkCTRL);
      %LinkedStack.add(%LinkCTRL);
      
      if(%Emitter.getLinkEmissionRotation())
         %LinkCTRL.setStateOn(true);
      else
         %LinkCTRL.setStateOn(false);
         
//-------------------------------------------------------

   %KeepAlignedStack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%KeepAlignedStack);
      
   %label = new GuiTextCtrl(){
   text = " Aligned Angle";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "100 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%KeepAlignedStack.add(%label);
      
       %AAOffset= new GuiTextEditCtrl()
      {
         class = AAOffsetCTRL;
         text = %Emitter.getFixedForceAngle();
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;
         tooltipprofile = "GuiToolTipProfile";
         ToolTip = "Aligned Angle Offset";
         tooltipWidth = "250";
         //dynamic
         EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%AAOffset);
      %KeepAlignedStack.add(%AAOffset);
      
      %KACTRL = new GuiCheckBoxCtrl()
      {
      class = KACtrl;      
      tooltipprofile = "GuiToolTipProfile";
      ToolTip = "Keep Aligned - Overrides Spin";
      tooltipWidth = "250";
      text ="";
      EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%KACTRL);
      %KeepAlignedStack.add(%KACTRL);
      
      if(%Emitter.getKeepAligned())
         %KACTRL.setStateOn(true);
      else
         %KACTRL.setStateOn(false);

//-------------------------------------------------------
%FAOffsetstack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%FAOffsetstack);
      
   %label = new GuiTextCtrl(){
   text = " Fixed Angle Offset ";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "120 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%FAOffsetstack.add(%label);

  
      %FA0Control= new GuiTextEditCtrl()
      {
         class = FixedAngleOffsetCTRL;
         text = %Emitter.getFixedForceAngle();
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;
         tooltipprofile = "GuiToolTipProfile";
         ToolTip = "Fixed Angle Offset";
         tooltipWidth = "250";
         //dynamic
         EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%FA0Control);
      %FAOffsetstack.add(%FA0Control);

//-------------------------------------------------------

%RAOffsetstack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%RAOffsetstack);
      
   %label = new GuiTextCtrl(){
   text = " Random Angle Offset ";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "120 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%RAOffsetstack.add(%label);

  
      %RA0Control= new GuiTextEditCtrl()
      {
         class = RandomAngleOffsetCTRL;
         text = %Emitter.getFixedForceAngle();
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;
         tooltipprofile = "GuiToolTipProfile";
         ToolTip = "Random Angle Offset";
         tooltipWidth = "250";
         //dynamic
         EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%RA0Control);
      %RAOffsetstack.add(%RA0Control);

//-------------------------------------------------------


%RandomArcstack = new GuiStackControl()
      {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         Extent = "235 32";
      };
      %stack.add(%RandomArcstack);
      
   %label = new GuiTextCtrl(){
   text = " Random Arc ";
   HorizSizing = "right";
   Profile = "GuiInspectorTextEditCenterProfile";
   Extent = "120 15";
   VertSizing = "center";
};
UIFlush.add(%label);
%RandomArcstack.add(%label);
  
      %RArcControl= new GuiTextEditCtrl()
      {
         class = RandomArcCTRL;
         text = %Emitter.getFixedForceAngle();
         Extent = "96 32";
         VertSizing = "top";
         Profile = GuiNumberEditProfile;
         tabComplete = true;
         tooltipprofile = "GuiToolTipProfile";
         ToolTip = "Random Arc";
         tooltipWidth = "250";
         //dynamic
         EmitterIndex = %EmitterIndex;
      };
      UIFlush.add(%RArcControl);
      %RandomArcstack.add(%RArcControl);

%ToCollapse = new SimSet();

      //Separate each graph field logically instead of doing so procedurally
      //type 0 = Base (Color, alpha)
      //type 1 = Base + Variation
      //type 2 = Base + variation + life
      
      %CatUI = %this.AddDataKeyCategory("Basics", %stack);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "LifeTime", 1);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "Quantity", 1);
      
      %RL = %CatUI.getParent();
      %stack.add(%RL);
      %ToCollapse.add(%RL);
      
      %CatUI = %this.AddDataKeyCategory("Motion", %stack);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "Speed", 2);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "Spin", 2);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "FixedForce", 2);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "RandomMotion", 2);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "EmissionForce", 1);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "EmissionAngle", 1);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "EmissionArc", 1);
      
      %RL = %CatUI.getParent();
      %stack.add(%RL);
      %ToCollapse.add(%RL);
      
      %CatUI = %this.AddDataKeyCategory("Appearance", %stack);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "SizeX", 2);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "SizeY", 2);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "RedChannel", 0);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "GreenChannel", 0);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "BlueChannel", 0);
      %this.BuildDataKeyUI(%CatUI, %asset, %EmitterIndex, "AlphaChannel", 0);
      
      %RL = %CatUI.getParent();
      %stack.add(%RL);
      %ToCollapse.add(%RL);

       %co = %ToCollapse.getCount();
      for(%itr=0;%itr<%co;%itr++)
      {
      %obj = %ToCollapse.getObject(%itr);
      %obj.collapse();
      }
      %Rollout.add(%stack);
        
   EmittersScroll.add(%Rollout);
      %Rollout.collapse();
      
   return(%ToCollapse);
}



function ParticleEditor::MinMaxSetup(%this, %parent, %asset, %linkCtrl, %Field, %Key_itr, %type)
{
         %MinMaxStack = new GuiStackControl()
         {
         StackingType = "Horizontal";
         HorizStacking = "Left to Right";
         //Profile = "GuiSunkenContainerProfile";
         };
         %parent.add(%MinMaxStack);
         UIFlush.add(%MinMaxStack);
         
         %MinStack = new GuiStackControl()
         {
         StackingType = "Vertical";
         HorizStacking = "Top to Bottom";
         Extent = "56 0";
         };
         %MinMaxStack.add(%MinStack);
         UIFlush.add(%MinStack);
         
            %MinTxt = new GuiTextCtrl()
            {
              text = "MIN";
              Extent = "32 16";       
              Profile = "GuiInspectorTextEditCenterProfile";
            };
            %MinStack.add(%MinTxt);
            UIFlush.add(%MinTxt);
            
            %Minval = new GuiTextEditCtrl()
            {
               class = "MinMaxTextEdit";
               text = mFloatLength(%asset.getMinValue(),2);
               Extent = "96 32";
               VertSizing = "top";
               tabComplete = true;
               Profile = "GuiTextEditProfile";
               //dynamic
               absmin = mFloatLength(%asset.getMinValue(),2);
               Identifier = "min";
               Link = %linkCtrl;
               Field = %Field;
               _Type = %type;
               _KeyIndex = %Key_itr;
            };
            %MinStack.add(%Minval);
            UIFlush.add(%Minval);
         
         %MaxStack = new GuiStackControl()
         {
         StackingType = "Vertical";
         HorizStacking = "Top to Bottom";
         Extent = "78 0";
         };
         %MinMaxStack.add(%MaxStack);
         UIFlush.add(%MaxStack);
            
            %MaxTxt = new GuiTextCtrl()
            {
              text = "MAX";
              Extent = "32 16";       
              Profile = "GuiInspectorTextEditCenterProfile";
            };
            %MaxStack.add(%MaxTxt);
            UIFlush.add(%MaxTxt);
            
            %Maxval = new GuiTextEditCtrl()
            {
               class = "MinMaxTextEdit";
               text = mFloatLength(%asset.getMaxValue(),2);
               Extent = "96 32";
               VertSizing = "top";
               tabComplete = true;
               Profile = GuiNumberEditProfile;
               //dynamic
               absmax = mFloatLength(%asset.getMaxValue(),2);
               Identifier = "max";
               Link = %linkCtrl;
               Field = %Field;
               _Type = %type;
               _KeyIndex = %Key_itr;
            };
            %MaxStack.add(%Maxval);
            UIFlush.add(%Maxval);
            
         %ValStack = new GuiStackControl()
         {
         StackingType = "Vertical";
         HorizStacking = "Top to Bottom";
         Extent = "78 0";
         };
         %MinMaxStack.add(%ValStack);
         UIFlush.add(%ValStack);
         
            %ValTxt = new GuiTextCtrl()
            {
              text = "Current";
              Extent = "32 16";       
              Profile = "GuiInspectorTextEditCenterProfile";
            };
            %ValStack.add(%ValTxt);
            UIFlush.add(%ValTxt);
            
         %Key = %asset.getDataKey(%Key_itr);
         %K_time = %Key._0;
         %K_value = %Key._1;

         
         
            %Curval = new GuiTextEditCtrl()
            {
               class = "MinMaxTextEdit";
               text = mFloatLength(%K_value,2);
               Extent = "96 32";
               VertSizing = "top";
               Profile = GuiNumberEditProfile;
               tabComplete = true;
               
               //dynamic
               Identifier = "val";
               Link = %linkCtrl;
               Field = %Field;
               _Type = %type;
               _KeyIndex = %Key_itr;
            };
            if(%type) %Curval.EmitterIndex = %asset;
            
            %ValStack.add(%Curval);
            UIFlush.add(%Curval);

return(%Curval);
}

function RemDKBTN::onAction(%this)
{
   %pfx = ParticleEditor.PFX;
   %asset = AssetDatabase.acquireAsset(%pfx.Particle);

   if(%this.isEmitter)
   {   
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   %Emitter.selectField(%this.FieldName);
   %count = %Emitter.getDataKeyCount();
   
   if((%count > 1) && (%this.DataKeyIndex != 0))
   {
      //Reorganize all tabs since we don't have a removeTab function in C++!!!!
      %Emitter.removeDataKey(%this.DataKeyIndex);
      %count--;
      ParticleEditor.schedule(0, repopulateTab, %this.TabTop, %Emitter, %this.EmitterIndex, %this.FieldName, %count, true, %this.TimeLineType);
   }
   else
   {
       echo("Cannot remove last data key in Field!");
       return;
   }
   }
   else
   {
   %asset.selectField(%this.Fieldname);
   %count = %asset.getDataKeyCount();
   
   if((%count > 1) && (%this.DataKeyIndex != 0))
   {
   //Reorganize all tabs since we don't have a removeTab function in C++!!!!

      %asset.removeDataKey(%this.DataKeyIndex);
      %count--;
      ParticleEditor.schedule(0, repopulateTab, %this.TabTop, %asset, %this.EmitterIndex, %this.FieldName, %count, false);
   }
   else
   {
       echo("Cannot remove last data key in Field!");
       return;
   }
   }
   
ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
}

function AddDKBTN::onAction(%this)
{
      %pfx = ParticleEditor.PFX;
      %asset = AssetDatabase.acquireAsset(%pfx.Particle);
    
   if(%this.isEmitter)
   {
   %Emitter = %asset.getEmitter(%this.EmitterIndex);
   %Emitter.selectField(%this.FieldName);
   
   %count = %Emitter.getDataKeyCount();

   %key = %Emitter.getDataKey(%this.DataKeyIndex);
   %K_time = %key._0;
   %K_value = %key._1;

   %type = %this.TimeLineType;

   if((%this.FieldName $= "RedChannel") || (%this.FieldName $= "BlueChannel") || (%this.FieldName $= "GreenChannel") || (%this.FieldName $= "AlphaChannel"))
   %timelineNormalize = 1;
   
   if(%type == 2)
   %timelineNormalize = 1;   
   
   %newtime = %K_time+0.01;

   if((%timelineNormalize) && (%newtime > 1.0))
   {
      echo("Valid Value range is 0.0 to 1.0. Stay Within the lines!");
      return;
   }
   
   %Nukey = %Emitter.addDataKey(%newtime, %K_value);
   
   for(%itr = 0; %itr < %count; %itr++)
   {
      if(%Nukey == %itr)
      {
         echo("Key" SPC %Nukey SPC "already exists at that time!");
         return;
      }
   }
  
   ParticleEditor.AddDataKeyTab(%Nukey, %this.TabTop, %Emitter, %this.EmitterIndex, %this.FieldName, %count+1, %type);
   ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());

   }
   else
   {
   %asset.selectField(%this.Fieldname);
   %count = %asset.getDataKeyCount();
   
   %key = %asset.getDataKey(%this.DataKeyIndex);
   %K_time = %key._0;
   %K_value = %key._1;
   
   %newtime = %K_time+0.01;

   %Nukey = %asset.addDataKey(%newtime, %K_value);
   
    for(%itr = 0; %itr < %count; %itr++)
   {
      if(%Nukey == %itr)
      {
         echo("Key" SPC %Nukey SPC "already exists at that time!");
         return;
      }
   }
        ParticleEditor.AddAssetDataKeyTab(%Nukey, %this.TabTop, %asset, %this.FieldName, %count+1);
         ParticleEditor.PFX.setParticleAsset(%asset.getAssetId());
      
   }
}


function ParticleEditor::repopulateTab(%this, %TabTop, %Emitter, %EmitterIndex, %FieldName, %count, %isEmitter, %type)
{
   %TabTop.deleteObjects();
   
   for(%itr = 0; %itr < %count; %itr++)
      {
         if(%isEmitter)
         ParticleEditor.AddDataKeyTab( %itr, %TabTop, %Emitter, %EmitterIndex, %FieldName, %count, %type);
         else
         ParticleEditor.AddAssetDataKeyTab( %itr, %TabTop, %Emitter, %FieldName, %count);
      }
}