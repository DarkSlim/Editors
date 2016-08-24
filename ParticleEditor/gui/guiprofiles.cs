if(!isObject(GuiInspectorTextEditProfile)) new GuiControlProfile ("GuiInspectorTextEditProfile")
{
   // Transparent Background
   opaque = true;
   fillColor = "0 0 0 0";
   fillColorHL = "255 255 255";

   // No Border (Rendered by field control)
   border = false;

   tab = true;
   canKeyFocus = true;

   // font
   fontType = "Arial";
   fontSize = 14;

   fontColor = "0 0 0";
   fontColorSEL = "43 107 206";
   fontColorHL = "244 244 244";
   fontColorNA = "100 100 100";
};

if(!isObject(GuiPFXHeaderProfile)) new GuiControlProfile (GuiPFXHeaderProfile)
{
   // Transparent Background
   opaque = true;
   
   border = true;   
   borderThickness = 2;
   
   tab = true;
   canKeyFocus = true;
   justify = "center";
   // font
   fontType = "Arial";
   fontSize = 14;

   fontColor = "0 0 0 255";
   fontColorSEL = "43 107 206";
   fontColorHL = "244 244 244";
   fontColorNA = "100 100 100";
};

if (!isObject(GuiSliderProfile)) new GuiControlProfile (GuiSliderProfile)
{
   fontColor ="1 1 1";
   fontSize = 18;
};

if (!isObject(GuiInspectorTextEditRightProfile)) new GuiControlProfile (GuiInspectorTextEditRightProfile : GuiInspectorTextEditProfile)
{
   justify = "right";
};

if (!isObject(GuiInspectorTextEditCenterProfile)) new GuiControlProfile (GuiInspectorTextEditCenterProfile : GuiInspectorTextEditRightProfile)
{
   justify = "center";
};

if (!isObject(GuiInspectorTextEditCenterProfile)) new GuiControlProfile (GuiInspectorTextEditCenterProfile : GuiInspectorTextEditRightProfile)
{
   justify = "center";
};

if (!isObject(GuiInspectorGroupProfile)) new GuiControlProfile (GuiInspectorGroupProfile )
{
   fontType    = "Arial";
   fontSize    = "18";
   
   fontColor = "255 255 255 150";
   fontColorHL = "25 25 25 220";
   fontColorNA = "128 128 128";
   
   justify = "left";
   opaque = true;
   border = true;
  
   bitmap = "./images/Inspector/rollout";
   
   textOffset = "30 -2";

};

if (!isObject(GuiInspectorFieldProfile)) new GuiControlProfile (GuiInspectorFieldProfile)
{
   // fill color
   opaque = false;
   fillColor = "255 255 255";
   fillColorHL = "128 128 128";
   fillColorNA = "244 244 244";

   // border color
   border = false;
   borderColor   = "190 190 190";
   borderColorHL = "156 156 156";
   borderColorNA = "64 64 64";
   
   bevelColorHL = "255 255 255";
   bevelColorLL = "0 0 0";

   // font
   fontType = "Arial";
   fontSize = 14;

   fontColor = "32 32 32";
   fontColorHL = "32 100 100";
   fontColorNA = "0 0 0";

   tab = true;
   canKeyFocus = true;
};

if (!isObject(GuiInspectorBackgroundProfile)) new GuiControlProfile (GuiInspectorBackgroundProfile : GuiInspectorFieldProfile)
{
   border = 5;
   cankeyfocus=true;
   tab = true;
};

if (!isObject(GuiInspectorTypeFileNameProfile)) new GuiControlProfile (GuiInspectorTypeFileNameProfile)
{
   // Transparent Background
   opaque = false;

   // No Border (Rendered by field control)
   border = 5;

   tab = true;
   canKeyFocus = true;

   // font
   fontType = "Arial";
   fontSize = 14;
   
   // Center text
   justify = "center";

   fontColor = "32 32 32";
   fontColorHL = "32 100 100";
   fontColorNA = "0 0 0";

   fillColor = "255 255 255";
   fillColorHL = "128 128 128";
   fillColorNA = "244 244 244";

   borderColor   = "190 190 190";
   borderColorHL = "156 156 156";
   borderColorNA = "64 64 64";
};

if (!isObject(InspectorTypeEnumProfile)) new GuiControlProfile (InspectorTypeEnumProfile : GuiInspectorFieldProfile)
{
   mouseOverSelected = true;
   bitmap = "./images/Inspector/scrollBar";
   hasBitmapArray = true;
   opaque=true;
   border=true;
   textOffset = "4 0";
};

if (!isObject(InspectorTypeCheckboxProfile)) new GuiControlProfile (InspectorTypeCheckboxProfile : GuiInspectorFieldProfile)
{
   bitmap = "./images/Inspector/checkBox";
   hasBitmapArray = true;
   opaque=false;
   border=false;
};
/*
if (!isObject(GuiToolboxButtonProfile)) new GuiControlProfile (GuiToolboxButtonProfile : GUIwi)
{
   justify = "center";
   fontColor = "0 0 0";
   border = 0;
   textOffset = "0 0";   
};
*/

if(!isObject(GuiScrollProfile)) new GuiControlProfile (GuiScrollProfile)
{
    opaque = true;
    fillColor = "255 255 255";
    border = 1;
    borderThickness = 2;
    bitmap = "./images/Inspector/scrollBar.png";
    hasBitmapArray = true;
};

//-----------------------------------------------------------------------------

if(!isObject(GuiTransparentScrollProfile)) new GuiControlProfile (GuiTransparentScrollProfile)
{
   opaque = false;
   fillColor = "255 255 255";
   border = true;
   borderThickness = 2;
   borderColor = "0 0 0";
   bitmap = "./images/Inspector/scrollBar.png";
   hasBitmapArray = true;
};