function onDropBegin(%filecount)
{
echo("Dropped" SPC %filecount SPC "files.");
}

//Called for each file

function onDropFile(%filename)
{
   
   %tmpassetname = fileBase(%filename); 
   %extension = fileExt(%filename);
   %newfilename =  %tmpassetname @ %extension;

   %patha = expandPath("^MyAssets/assets");

   switch$(%extension){
      case ".png": %pathb="/images/";
                  %assettype = 0;
      case ".ogg": %pathb="/audio/";
                  %assettype = 1;
      case ".wav": %pathb="/audio/";
                  %assettype = 1;
      default: warn("Not a valid file type");
               return;
   }

   %fo = new FileStreamObject();
   %ftarget =  new FileStreamObject();
   
   %fullpath = %patha@%pathb@%newfilename;   
   
   %result = %ftarget.open(%fullpath, "write");
   
   if(!%result)
   {
      warn("Cannot create file" SPC %fullpath @ ". Aborting.");
      return;
   }
   
   if(%fo.open(%filename, "read"))
   {
      %ftarget.copyFrom(%fo);
   }
   else
   {
      warn("Cannot open file" SPC %filename @". Aborting.");
      %ftarget.close();
   }   

   %ftarget.close();
   %fo.close();
   
   switch(%assettype){
      case 0 : createImageAssetFromDrop(%tmpassetname, %patha@%pathb@fileBase(%filename)@".asset.taml", %fullpath);
      case 1 : createAudioAssetFromDrop(%tmpassetname, %patha@%pathb@fileBase(%filename)@".asset.taml", %fullpath);
   }
    
}

function createImageAssetFromDrop(%assetname, %assetfile, %imagefile)
{
   %imgasset = new ImageAsset()
   {
      ImageFile =  %imagefile;
      CellWidth = 1;
      CellHeight = 1;
      FilterMode = "NEAREST";
   };
   AssetDatabase.addPrivateAsset(%imgasset);
   
  IMGAssetNameGUI.setText(%assetname @ "_IMG");

   ImageSlicer.imageslicerprep(%imgasset);
}

function createAudioAssetFromDrop(%assetname, %assetfile, %soundfile)
{
%sndasset = new AudioAsset(){
   AssetName = %assetname;   
   AudioFile = %soundfile;
   };

//Not much else to set up for Audio Assets.
TamlWrite(%sndasset,%assetfile);

%def = ModuleDatabase.findModule(MyAssets,1);
   AssetDatabase.addDeclaredAsset(%def, %assetfile);
   AssetDatabase.acquireAsset("MyAssets:"@%assetname);
   
   AssetDatabase.refreshAllAssets();
}


function onDropEnd()
{

}