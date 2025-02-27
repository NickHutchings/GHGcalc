#define WIDE_AREA
using System;
using System.Collections.Generic;
using System.Xml;

//simulates the crop processes
public class CropClass
{
    public  List<manure> theManureApplied;
    public string parens;
    public struct fertRecord
    {
        public string Name;
        public string Parens;
        public double Namount;
        public int Month_applied;
        public int dayOfApplication;
        public timeClass applicdate;
        public string Applic_techniqueS;
        public int Applic_techniqueI;
        public int ManureStorageID;
        public int speciesGroup;
        public string Units;
        //Check to see if fertiliser or manure is applied outside the crop period
        public double getNamount() { return Namount; }
        public string getName() { return Name; }
        public int getspeciesGroup() { return speciesGroup; }
        public int getManureType() { return ManureStorageID; }
        public int GetMonth_applied() { return Month_applied; }
        public void SetdayOfApplication(int aDay) { dayOfApplication = aDay; }
        public int GetdayOfApplication() { return dayOfApplication; }
        public void SetNamount(double aVal) { Namount = aVal; }
        public void SetapplicDate(int day, int month, int year) 
        {
            applicdate.setDate(day, month, year);
        }
        public timeClass GetDate() { return applicdate; }
        public long GetRelativeDay(long startDay)//timeClass startDate) 
        {
            long retVal = applicdate.getLongTime() - startDay;// startDate.getLongTime();
      
            return retVal;
        }
    }
    public double NCrop;
    string name;
    int identity;
    //inputs
    double area;
    public timeClass theStartDate;
    timeClass theEndDate;
    bool isIrrigated;

    string cropSequenceName;
    int cropSequenceNo;
    public List<fertRecord> fertiliserApplied;
    public List<fertRecord> manureApplied;
    
    //parameters
    double [] propAboveGroundResidues = new double[2];
    double propBelowGroundResidues;
    double CconcBelowGroundResidues;
    double CtoNBelowGroundResidues;
    double NDepositionRate;
    double urineNH3EmissionFactor;
    double manureN2OEmissionFactor;
    double fertiliserN2OEmissionFactor;
    double soilN2OEmissionFactor;
    double soilN2Factor;
    string harvestMethod;
    double MaximumRootingDepth;
    double NfixationFactor;
    double baseTemperature;
    long duration = 0;
    bool permanent = false;
    double droughtSusceptability = 1.0;
    bool zeroGasEmissionsDebugging = false;
    bool zeroLeachingDebugging = false;
    double irrigationThreshold = 0.5;
    double irrigationMinimum = 5.0;
    double[] potentialEvapoTrans = new double[366];
    double[] precipitation = new double[366];
    double[] temperature = new double[366];
    double[] Tsum = new double[366];
    double[] potEvapoTrans;
    double[] evaporation;
    double[] drainage;
    double[] transpire;
    double[] soilWater;
    double[] plantavailableWater;
    double[] LAI;
    double[] irrigationWater;
    double[] droughtFactorPlant;
    List<GlobalVars.product> theProducts = new List<GlobalVars.product>();

#if WIDE_AREA
    double AG_slope = 0;
    double AG_intercept = 0;

#endif

    //other variables
    //variables to output
    double CFixed = 0;//carbon fixed
    double surfaceResidueC;
    double subsurfaceResidueC;
    double urineC = 0;
    double faecalC = 0;
    double grazedC = 0;
    double grazingCH4C = 0;
    double harvestedC = 0;
    double harvestedDM = 0;
    double storageProcessingCLoss = 0;
    double storageProcessingNLoss = 0;
    double storageProcessingDMLoss = 0;

    double NyieldMax = 0;
    double maxCropNuptake = 0;
    double modelledCropNuptake = 0;
    double NavailFact = 0;
    double Nfixed = 0;
    double nAtm = 0;
    double manureNH3emission = 0;
    double fertiliserNH3emission = 0;
    double urineNH3emission = 0;
    double surfaceResidueN;
    double subsurfaceResidueN;
    double surfaceResidueDM = 0;
    double fertiliserC = 0;
    double mineralNavailable = 0;
    double residueN = 0;
    double excretaNInput = 0;
    double excretaCInput = 0;
    double fertiliserNinput = 0;
    double harvestedN = 0;
    double grazedN = 0;
    double totalManureNApplied = 0;
    double N2ONemission = 0;
    double N2Nemission = 0;
    double cropNuptake = 0;
    double manureFOMn = 0;
    double manureHUMn = 0;
    double urineNasFertilizer = 0;
    double faecalN = 0;
    double burntResidueC = 0;
    double burntResidueN = 0;
    double burningN2ON = 0;
    double burningNH3N = 0;
    double burningNOxN = 0;
    double burningCOC = 0;
    double burningCO2C = 0;
    double burningBlackC = 0;
    double burningOtherN = 0;
    double OrganicNLeached = 0;
    double soilNMineralisation = 0;
    double mineralNFromLastCrop = 0;
    double mineralNToNextCrop = 0;
    double residueCfromLastCrop = 0;
    double residueNfromLastCrop = 0;
    double residueCtoNextCrop = 0;
    double residueNtoNextCrop = 0;
    double fertiliserN2OEmission = 0;
    double manureN2OEmission;
    double cropResidueN2O = 0;
    double soilN2OEmission = 0;
    double proportionLeached = 0;
    double mineralNreserve = 0;
    double nitrateLeaching = 0;
    double totalTsum = 0;
    double maxLAI = 5.0;

    //other variables not for output
    public double [] manureFOMCsurface;
    double [] manureHUMCsurface;
    double [] manureFOMCsubsurface;
    double [] manureHUMCsubsurface;
    double[] manureFOMNsurface;
    double[] manureHUMNsurface;
    double[] manureTAN;
    double[] fertiliserN;
    //! nitrification inhibitor - 1 at time of application
    double[] nitrificationInhibitor;

    GlobalVars.product residueFromPrevious;
    GlobalVars.product residueToNext;

#if WIDE_AREA
    double F_CR = 0;
    double FRAC_Remove = 0;
    double CropResidueN2OEmission = 0;
    public void SetFRAC_Remove(double aVal) { FRAC_Remove = aVal; }
    public double GetCropResidueN2OEmission() { return CropResidueN2OEmission; }
#endif
    public void setsoilN2Factor(double aVal) { soilN2Factor = aVal; }
    public void SetcropSequenceNo(int aVal) { cropSequenceNo = aVal; }
    public double getCropResidueN2O() { return cropResidueN2O; }
    public double getNFix() { return Nfixed; }
    public double getnAtm() { return nAtm; }
    public double getArea() { return area; }
    public double GetpropBelowGroundResidues() { return propBelowGroundResidues; }
    public double GetCconcBelowGroundResidues() { return CconcBelowGroundResidues; }

    public double GetgrazingCH4C() { return grazingCH4C; }
    public double GetfaecalC() { return faecalC; }
    public double GetfaecalN() { return faecalN; }
    public double GetresidueN() { return residueN; }
    public double GettotalManureNApplied() { return totalManureNApplied; }
    public double GetexcretaNInput() { return excretaNInput; }
    public double GetexcretaCInput() { return excretaCInput; }
    public double GetmanureNH3Nemission() { return manureNH3emission; }
    public double GetN2ONemission() { return N2ONemission; }
    public double GetN2Nemission() { return N2Nemission; }
    public double GetfertiliserNH3Nemission() { return fertiliserNH3emission; }
    public double GetmineralNavailable() { return mineralNavailable; }
    public double GetfertiliserN2ONEmission() { return fertiliserN2OEmission; }
    public double GetmanureN2ONEmission() { return manureN2OEmission; }
    public double GetcropResidueN2ON() { return cropResidueN2O; }
    public double GetsoilN2ONEmission() { return soilN2OEmission; }

    public double GetburningN2ON() { return burningN2ON; }
    public double GetburningNH3N() { return burningNH3N; }
    public double GetburningNOxN() { return burningNOxN; }
    public double GetburningOtherN() { return burningOtherN; }
    public double GetburningCOC() { return burningCOC; }
    public double GetburningCO2C() { return burningCO2C; }
    public double GetburningBlackC() { return burningBlackC; }
    public double GetburntResidueC() { return burntResidueC; }
    public double GetburntResidueN() { return burntResidueN; }
    public double GetharvestedN() { return harvestedN; }
    public double GetmineralNToNextCrop() { return mineralNToNextCrop; }
    //public bool GetContinues() { return continues; }
    public double GetCropNuptake() { return cropNuptake; }

    public List<GlobalVars.product> GettheProducts() { return theProducts; }
    public List<fertRecord> GetfertiliserApplied() { return fertiliserApplied; }
    public List<fertRecord> GetmanureApplied() { return manureApplied; }
    public int GetStartDay() { return theStartDate.GetDay(); }
    public int GetEndDay() { return theEndDate.GetDay(); }
    public int GetStartMonth() { return theStartDate.GetMonth(); }
    public int GetEndMonth() { return theEndDate.GetMonth(); }
    public int GetEndYear() { return theEndDate.GetYear(); }
    public int GetStartYear() { return theStartDate.GetYear(); }
    public void SetStartYear(int aVal) { theStartDate.SetYear(aVal); }
    public void SetEndYear(int aVal) { theEndDate.SetYear(aVal); }
    public bool Getpermanent() {return permanent;}

    public double GetMaximumRootingDepth() { return MaximumRootingDepth; }
    public double GetmanureFOMCsurface(int day) { return manureFOMCsurface[day]; }
    public double GetmanureFOMNsurface(int day) { return manureFOMNsurface[day]; }
    public double GetmanureHUMCsurface(int day) { return manureHUMCsurface[day]; }
    public long getDuration() { return duration; }
    public double GetOrganicNLeached() {return OrganicNLeached; }
    public double GetSoilNMineralisation() { return soilNMineralisation; }
    
    public double GetproportionLeached() { return proportionLeached; }
    public double GetMineralNreserve() { return mineralNreserve; }
    
    public void SetnitrateLeaching(double aVal){nitrateLeaching=aVal;}
    public double GetnitrateLeaching() { return nitrateLeaching; } 
    public double GetManureNapplied()
    {
        double retVal = 0;
        for (int i = 0; i < manureApplied.Count; i++)
            retVal += manureApplied[i].getNamount();
        return retVal;
    }
    private CropClass(){}

    public long getStartLongTime() { return theStartDate.getLongTime(); }
    public long getEndLongTime(){return theEndDate.getLongTime();}

    public CropClass(string path, int index, int zoneNr, string theCropSeqName)
    {
        fertiliserApplied = new List<fertRecord>();
        manureApplied = new List<fertRecord>();
        cropSequenceName = theCropSeqName;
        FileInformation cropInformation = new FileInformation(GlobalVars.Instance.getFarmFilePath());
        cropInformation.setPath(path+"("+index+")");
        name = cropInformation.getItemString("NameOfCrop");
        string tempString = cropInformation.getItemString("Irrigation");
        if (tempString == "false")
            isIrrigated = false;
        else
            isIrrigated = true;
        if (name != "Bare soil")
            GetCropInformation(path, index, zoneNr);
        int length = name.IndexOf("Permanent");
        if (length != -1)
        {
            permanent = true;
            droughtSusceptability = 0.5;
        }
        int minFertiliserApplied = 99, maxFertiliserApplied = 0;
        cropInformation.PathNames.Add("Fertilizer_applied");
        cropInformation.getSectionNumber(ref minFertiliserApplied, ref maxFertiliserApplied);
        for (int i = minFertiliserApplied; i <= maxFertiliserApplied; i++)
        {
            if (cropInformation.doesIDExist(i))
            {
                fertRecord newFertRecord = new fertRecord();
                newFertRecord.applicdate = new timeClass();
                cropInformation.Identity.Add(i);
                newFertRecord.Name = cropInformation.getItemString("Name");
                newFertRecord.Units = cropInformation.getItemString("Unit");
                newFertRecord.Namount = cropInformation.getItemDouble("Value");
                cropInformation.PathNames.Add("Applic_technique_Fertilizer");
                cropInformation.Identity.Add(-1);
                newFertRecord.Applic_techniqueS = cropInformation.getItemString("NameS");
                //newFertRecord.Applic_techniqueI = cropInformation.getItemInt("NameI");
                fertiliserApplied.Add(newFertRecord);
                cropInformation.PathNames.RemoveAt(cropInformation.PathNames.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
            }
        }

        int minManure_applied = 99, maxManure_applied = 0;
        cropInformation.PathNames[cropInformation.PathNames.Count - 1] = "Manure_applied";
        cropInformation.getSectionNumber(ref minManure_applied, ref maxManure_applied);
        //Check to see if manure is applied outside crop period. Why does this code not use ReadFertManApplication? NJH
        for (int i = minManure_applied; i <= maxManure_applied; i++)
        {
            if (cropInformation.doesIDExist(i))
            {
                fertRecord newFertRecord = new fertRecord();
                newFertRecord.applicdate = new timeClass();
                cropInformation.Identity.Add(i);
                //newFertRecord.Name = cropInformation.getItemString("Name");
                newFertRecord.Units = cropInformation.getItemString("Unit");
                newFertRecord.Namount = cropInformation.getItemDouble("Value");
                newFertRecord.ManureStorageID = cropInformation.getItemInt("StorageType");
                //newFertRecord.speciesGroup = cropInformation.getItemInt("SpeciesGroup");
                cropInformation.PathNames.Add("Applic_technique_Manure");
                cropInformation.Identity.Add(-1);
                newFertRecord.Applic_techniqueS = cropInformation.getItemString("NameS");
                //newFertRecord.Applic_techniqueI = cropInformation.getItemInt("NameI");
                manureApplied.Add(newFertRecord);
                cropInformation.PathNames.RemoveAt(cropInformation.PathNames.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
            }
        }
        getParameters(index, zoneNr, path);
    }

    public void GetCropInformation(string path, int index, int zoneNr)
    {
        FileInformation cropInformation = new FileInformation(GlobalVars.Instance.getFarmFilePath());
        cropInformation.setPath(path + "(" + index + ")");
        cropInformation.Identity.Add(-1);
        cropInformation.PathNames.Add("HarvestMethod");
        harvestMethod = cropInformation.getItemString("Value");
        cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
        cropInformation.PathNames[cropInformation.PathNames.Count - 1] = "Product";
        int minProduct = 99, maxProduct = 0;
        cropInformation.getSectionNumber(ref minProduct, ref maxProduct);
        double totProduction = 0;
        double isGrazedProduction = 0;
        for (int i = minProduct; i <= maxProduct; i++)
        {
            if (cropInformation.doesIDExist(i) == true)
            {
                cropInformation.Identity.Add(i);
                string cropPath = path + "(" + index + ")" + ".Product";
                GlobalVars.product anExample = new GlobalVars.product();
                feedItem aComposition = new feedItem(cropPath, i, false, parens + "_" + i.ToString());
                anExample.composition = aComposition;
                //anExample.Harvested = cropInformation.getItemString("Harvested");

                /*bool checkIncorp = anExample.composition.GetName().Contains("Incorporated");
                if (checkIncorp)
                    anExample.Harvested = "Incorporated";
                */
                string temp = path + "(" + index + ")" + ".Product" + "(" + i.ToString() + ").Potential_yield(-1)";
                anExample.Potential_yield = cropInformation.getItemDouble("Value", temp);
                if (anExample.Potential_yield <= 0)
                {
                    string messageString = ("Error - potential production of a crop product cannot be zero or less than zero\n");
                    messageString += ("Crop source = " + path + "\n");
                    messageString += ("Crop name = " + name);
                    GlobalVars.Instance.Error(messageString);
                }
                totProduction += anExample.Potential_yield;
                /*temp = path + "(" + index + ")" + ".Product" + "(" + i.ToString() + ").Expected_yield(-1)";
                anExample.Expected_yield = cropInformation.getItemDouble("Value", temp);
                totProduction += anExample.Potential_yield;
                if (anExample.composition.GetisGrazed() == true || anExample.Harvested.Contains("Residue"))
                {
                    anExample.Grazed_yield = anExample.Expected_yield;
                    isGrazedProduction += anExample.Expected_yield;
                }
                else
                    if (!anExample.Harvested.Contains("Incorporated"))
                        anExample.composition.AdjustAmount(1+anExample.composition.GetStoreProcessFactor());*/
                if (anExample.Potential_yield > 0)
                    theProducts.Add(anExample);
                cropInformation.PathNames.RemoveAt(cropInformation.PathNames.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
                cropInformation.Identity.RemoveAt(cropInformation.Identity.Count - 1);
            }
        }

        if ((totProduction == 0) && (name != "Bare soil"))
        {

            string messageString = ("Error - total potential production of a crop cannot be zero\n");
            messageString += ("Crop source = " + path + "\n");
            messageString += ("Crop name = " + name);
            GlobalVars.Instance.Error(messageString);
        }
    }

    public void getParameters(int cropIdentityNo, int zoneNR, string cropPath)
    {
        identity = cropIdentityNo;
        FileInformation cropParamFile = new FileInformation(GlobalVars.Instance.getParamFilePath());  //prob here
        string basePath = "AgroecologicalZone(" + zoneNR + ")";
        string tmpPath;
        NDepositionRate = GlobalVars.Instance.theZoneData.GetNdeposition();
        tmpPath = basePath + ".UrineNH3EF(-1)";
        cropParamFile.setPath(tmpPath);
        if (zeroGasEmissionsDebugging)
            urineNH3EmissionFactor = 0;
        else
            urineNH3EmissionFactor = cropParamFile.getItemDouble("Value");

        cropParamFile.Identity.RemoveAt(cropParamFile.Identity.Count - 1);
        if (name != "Bare soil")
        {

            cropParamFile.PathNames[cropParamFile.PathNames.Count - 1] = "Crop";
            int min = 99, max = 0;
            cropParamFile.getSectionNumber(ref min, ref max);
            bool gotit = false;
            string aname ="None";
            for (int j = min; j <= max; j++)
            {
                if (cropParamFile.doesIDExist(j))
                {
                    tmpPath = basePath + ".Crop" + "(" + j.ToString() + ")";
                    cropParamFile.setPath(tmpPath);
                    aname = cropParamFile.getItemString("Name");
         
                    if (aname == name)
                    {
                        gotit = true;
                        break;
                    }
                    cropParamFile.Identity.RemoveAt(cropParamFile.Identity.Count - 1);
                }
            }
            if (gotit == false)
            {
              
                string message1=("Error - could not find crop in parameter file\n");
                message1 += message1 + ("Crop source = " + cropPath + "(" + cropIdentityNo.ToString() + ")\n");
                message1+=("Crop name = " + name);
                GlobalVars.Instance.Error(message1);
            }
            
            cropParamFile.setPath(tmpPath + ".MaxLAI(-1)");
            double value = cropParamFile.getItemDouble("Value", false);
            if (value!=0)
                maxLAI = value;
          
            cropParamFile.setPath(tmpPath + ".NavailMax(-1)");
            cropParamFile.setPath(tmpPath + ".NfixationFactor(-1)");
            NfixationFactor = cropParamFile.getItemDouble("Value");
            
            cropParamFile.setPath(tmpPath + ".PropBelowGroundResidues(-1)");
            propBelowGroundResidues = cropParamFile.getItemDouble("Value");
            cropParamFile.setPath(tmpPath + ".BelowGroundCconc(-1)");
            CconcBelowGroundResidues = cropParamFile.getItemDouble("Value");
            cropParamFile.setPath(tmpPath + ".BelowGroundCtoN(-1)");
            CtoNBelowGroundResidues = cropParamFile.getItemDouble("Value");
            //at this point, tmpPath = "AgroecologicalZone(1).Crop(20)"
            // PathNames are “AgroecologicalZone” and “Crop", Identity = “1”, “20” and “-1”
            cropParamFile.setPath(tmpPath + ".MaximumRootingDepth(-1)");
            // PathNames are “AgroecologicalZone”, “Crop" and “MaximumRootingDepth”, Identity = “1”, “20” and “-1”
            MaximumRootingDepth = cropParamFile.getItemDouble("Value");
            cropParamFile.setPath(tmpPath + ".Irrigation(-1).irrigationThreshold(-1)");
            irrigationThreshold= cropParamFile.getItemDouble("Value", false);
            // PathNames are “AgroecologicalZone”, “Crop", “Irrigation” and “irrigationThreshold”, Identity = “1”, “20”, “-1” and “-1”
            cropParamFile.setPath(tmpPath + ".Irrigation(-1).irrigationMinimum(-1)");
            irrigationMinimum = cropParamFile.getItemDouble("Value", false);

            cropParamFile.Identity.RemoveAt(cropParamFile.Identity.Count - 1);
            cropParamFile.Identity.RemoveAt(cropParamFile.Identity.Count - 1);
            // PathNames are “AgroecologicalZone”, “Crop", “Irrigation” and “irrigationThreshold”, Identity = “1” and “20”
            cropParamFile.PathNames.RemoveAt(cropParamFile.PathNames.Count - 1);
            // PathNames are “AgroecologicalZone”, “Crop" and “Irrigation”, Identity = “1” and “20”

#if WIDE_AREA
            cropParamFile.setPath(tmpPath + ".WideArea(-1).Residue_slope(-1)");
            AG_slope = cropParamFile.getItemDouble("Value", true);
            cropParamFile.setPath(tmpPath + ".WideArea(-1).Residue_intercept(-1)");
            AG_intercept = cropParamFile.getItemDouble("Value", true);
#endif

            cropParamFile.PathNames[cropParamFile.PathNames.Count - 1] = "HarvestMethod";
            // PathNames are “AgroecologicalZone”, “Crop" and “HarvestMethod”, Identity = “1” and “20”
            max = 0;
            min = 999;
            cropParamFile.getSectionNumber(ref min, ref max);
            cropParamFile.Identity.Add(-1);
            // PathNames are “AgroecologicalZone”, “Crop" and “HarvestMethod”, Identity = “1”, “20” and “-1”
            for (int i = min; i <= max; i++)
            {
                //    if (cropParamFile.doesIDExist(i))
                {
                    cropParamFile.Identity[cropParamFile.PathNames.Count - 1] = i;
                    string harvestMethodName = cropParamFile.getItemString("Name");
                    if (harvestMethodName == harvestMethod)
                    {
                        cropParamFile.PathNames.Add("PropAboveGroundResidues");
                        cropParamFile.Identity.Add(-1);
                        if (harvestMethodName == "Grazing")
                            propAboveGroundResidues[1] = cropParamFile.getItemDouble("Value");
                        else
                            propAboveGroundResidues[0] = cropParamFile.getItemDouble("Value");
                        break;
                    }
                }
            }
        }
    }


    public double CalculateCropNUptake()
    {
        double uptakeN = 0;
        double NinProduct = 0;
        double NinSurfaceResidues = 0;
        double NinSubsurfaceResidues = 0;
        for (int i = 0; i < theProducts.Count; i++)
        {
            if (theProducts[i].composition.GetisGrazed()) //grazed crop
            {
                NinProduct += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc();
                NinSurfaceResidues += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc() * propAboveGroundResidues[1];
            }
            else
            {
                //ungrazed part of crop
                NinProduct += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc();
                NinSurfaceResidues += theProducts[i].GetExpectedYield() * theProducts[i].composition.GetN_conc() * propAboveGroundResidues[0];
            }
            double adjustment = 1.0;// (1 - 0.5) / (theProducts[i].GetExpectedYield() / theProducts[i].GetPotential_yield());
            NinSubsurfaceResidues += theProducts[i].GetExpectedYield() * (GetCconcBelowGroundResidues() * GetpropBelowGroundResidues() * adjustment) / CtoNBelowGroundResidues;
        }
        uptakeN += NinProduct + NinSurfaceResidues + NinSubsurfaceResidues;
        return uptakeN;
    }

    public double CalculateMaxCropNUptake()
    {
        double uptakeN = 0;
        double NinProduct = 0;
        double NinSurfaceResidues = 0;
        double NinSubsurfaceResidues = 0;
        for (int i = 0; i < theProducts.Count; i++)
        {
            if (theProducts[i].composition.GetisGrazed()) //grazed crop
            {
                NinProduct += theProducts[i].GetPotential_yield() * theProducts[i].composition.GetN_conc();
                NinSurfaceResidues += theProducts[i].GetPotential_yield() * theProducts[i].composition.GetN_conc() * propAboveGroundResidues[1];
                NinSubsurfaceResidues += theProducts[i].GetPotential_yield() * (GetCconcBelowGroundResidues() * GetpropBelowGroundResidues()) / CtoNBelowGroundResidues;
            }
            else
            {
                //ungrazed part of crop
                NinProduct += theProducts[i].GetPotential_yield() * theProducts[i].composition.GetN_conc();
                NinSurfaceResidues += theProducts[i].GetPotential_yield() * theProducts[i].composition.GetN_conc() * propAboveGroundResidues[0];
                NinSubsurfaceResidues += theProducts[i].GetPotential_yield() * (GetCconcBelowGroundResidues() * GetpropBelowGroundResidues()) / CtoNBelowGroundResidues;
            }
        }
        uptakeN += NinProduct + NinSurfaceResidues + NinSubsurfaceResidues;
        return uptakeN;
    }

    public void CalculateCropResidues()
    {
        double AGR = 0;
        double N_AG = 0;
        double FRAC_Renew = 1;
        double BGR = 0;
        double RS = 0;
        double R = 0;
        surfaceResidueN = 0;
        subsurfaceResidueN = 0;
        double N_BG =GetCconcBelowGroundResidues() / CtoNBelowGroundResidues;
        double Crop_t = 0;
        for (int i = 0; i < theProducts.Count; i++)
        {
            Crop_t += theProducts[i].GetPotential_yield();
            N_AG += theProducts[i].composition.GetN_conc() * theProducts[i].GetPotential_yield();
        }
        N_AG /= Crop_t;
        AGR = (AG_slope * Crop_t + AG_intercept) * FRAC_Renew;
        BGR = (Crop_t + AGR) * RS * FRAC_Renew;
        surfaceResidueN = AGR * N_AG * (1 - FRAC_Remove);
        subsurfaceResidueN = BGR * N_BG;
        F_CR = surfaceResidueN + subsurfaceResidueN;
    }

    public void CalculateManureInput(bool lockit)
    {
        manureNH3emission = 0;
#if WIDE_AREA
        totalManureNApplied= 0;//in kg/ha
        for (int i = 0; i < manureApplied.Count; i++)
        {
            totalManureNApplied += manureApplied[i].getNamount();
            int ManureType = manureApplied[i].getManureType();
            int maxManure = 0;
            maxManure = GlobalVars.Instance.theZoneData.theFertManData.Count;
            bool gotit = false;
            int j = 0;
            while ((j < maxManure)&(!gotit))
            {
                int tmpType = GlobalVars.Instance.theZoneData.theFertManData[j].manureType;  //type of manure storage
                int tmpSpecies = GlobalVars.Instance.theZoneData.theFertManData[j].speciesGroup;
                bool isManureTypeSame;
                if (ManureType == 1 && tmpType == 2)
                    isManureTypeSame = true;
                else if (ManureType == 2 && tmpType == 1)
                    isManureTypeSame = true;
                else if (ManureType == 3 && tmpType == 4)
                    isManureTypeSame = true;
                else if (ManureType == 4 && tmpType == 3)
                    isManureTypeSame = true;
                else if (ManureType == 6 && tmpType == 9)
                    isManureTypeSame = true;
                else if (ManureType == 9 && tmpType == 6)
                    isManureTypeSame = true;
                else if (ManureType == 7 && tmpType == 10)
                    isManureTypeSame = true;
                else if (ManureType == 10 && tmpType == 7)
                    isManureTypeSame = true;
                else if (ManureType == 8 && tmpType == 12)
                    isManureTypeSame = true;
                else if (ManureType == 12 && tmpType == 8)
                    isManureTypeSame = true;
                else if (ManureType == 13 && tmpType == 14)
                    isManureTypeSame = true;
                else if (ManureType == 14 && tmpType == 13)
                    isManureTypeSame = true;
                else
                    isManureTypeSame = false;
                if (isManureTypeSame)
                {
                    gotit = true;
                    manureNH3emission += manureApplied[i].getNamount() * GlobalVars.Instance.theZoneData.theFertManData[j].EFNH3FieldTier2;//Tier 2 EF is proportion of total N
                }
                else
                    j++;
            }
            if (!gotit)
            {
                string messageString = ("Error - unable to find ammonia emission factor for a manure\n");
                Console.Write(messageString);
                Console.ReadKey();
            }
        }
        manureN2OEmissionFactor = GlobalVars.Instance.theZoneData.getmanureN20EmissionFactor();
        manureN2OEmission += manureN2OEmissionFactor * totalManureNApplied;
#endif
    }

    public void CalculateFertiliserInput()
    {
        double fertiliserNin = 0;
        fertiliserNH3emission = 0;
        fertiliserNinput = 0;
        fertiliserN2OEmissionFactor = GlobalVars.Instance.theZoneData.getfertiliserN20EmissionFactor();

        FileInformation cropInformation = new FileInformation(GlobalVars.Instance.getfertManFilePath());
#if WIDE_AREA
        double Napplied = 0;
        double NH3EmissionFactor = 0;

        for (int i = 0; i < fertiliserApplied.Count; i++)
        {

            if (fertiliserApplied[i].getName() != "Nitrification inhibitor")
                Napplied = fertiliserApplied[i].getNamount();

            fertiliserNinput += Napplied;
            string fertilizerName = fertiliserApplied[i].getName();
            cropInformation.setPath("AgroecologicalZone(" + GlobalVars.Instance.GetZone().ToString() + ").fertiliser");
            int max = 0;
            int min = 99;
            cropInformation.getSectionNumber(ref min, ref max);
            cropInformation.Identity.Add(min);
            bool found = false;
            for (int j = min; j <= max; j++)
            {
                cropInformation.Identity[1] = j;
                string fertManName = cropInformation.getItemString("Name");
                if (fertManName.CompareTo(fertilizerName) == 0)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
                GlobalVars.Instance.Error("Fertiliser not found in FertMan file for " + fertilizerName);
            cropInformation.PathNames.Add("Cconcentration");
            cropInformation.Identity.Add(-1);
            double Cconc = cropInformation.getItemDouble("Value");
            cropInformation.PathNames[2] = "Nconcentration";
            double Nconc = cropInformation.getItemDouble("Value");
            double amount = 0;
            if (Nconc > 0)
                amount = Napplied / Nconc;
            fertiliserNin += Napplied;
            fertiliserC += amount * Cconc;

            int maxFert = 0;
            maxFert = GlobalVars.Instance.theZoneData.theFertManData.Count;
            found = false;
            for (int j = 0; j < maxFert; j++)
            {
                string tmpName = GlobalVars.Instance.theZoneData.theFertManData[j].name;
                if (tmpName == fertilizerName)
                {
                    NH3EmissionFactor = GlobalVars.Instance.theZoneData.theFertManData[j].fertManNH3EmissionFactor;
                    found = true;
                    break;
                }
            }            
        }
        fertiliserNH3emission += NH3EmissionFactor * Napplied;
        fertiliserN2OEmission += fertiliserN2OEmissionFactor * Napplied;

#endif
    }

    public double CalculateNitrificationInhibitor()
    {
        double retVal = 0;
        for (int i = 0; i < duration; i++)
        {
            if (i > 0)
            {
                if (nitrificationInhibitor[i] < nitrificationInhibitor[i - 1])  //no new inhibitor applied
                {
                    double temp = temperature[i];
                    double degRate = Math.Log(2) / (168 * Math.Exp(-0.084 * temp));//from F.M. Kelliher a,b,*, T.J. Clough b, H. Clark c, G. Rys d, J.R. Sedcole b
                    nitrificationInhibitor[i] = nitrificationInhibitor[i - 1] * Math.Exp(-degRate);
                    if (nitrificationInhibitor[i] < 0.000001)
                        nitrificationInhibitor[i] = 0;
                }
            }
            retVal += nitrificationInhibitor[i];
        }
        retVal /= duration;
        return retVal;
    }

    public double GetNfixation(double deficit)
    {
        double retVal = 0;
        if ((deficit>0)&&(NfixationFactor>=0))
            retVal = deficit * NfixationFactor;
        return retVal;
    }

    public double GetcumulativePrecipitation()
    {
        double cumPrecip = 0;
        for (int i = 0; i < (int)duration; i++)
            cumPrecip += precipitation[i];
        return cumPrecip;
    }

    public double GetcumulativeDrainage()
    {
        double cum = 0;
        for (int i = 0; i < (int)duration; i++)
            cum += drainage[i];
        return cum;
    }

    public double GetcumulativepotEvapoTrans()
    {
        double cum = 0;
        for (int i = 0; i < (int)duration; i++)
            cum += potentialEvapoTrans[i];
        return cum;
    }

    public double GetcumulativeEvaporation()
    {
        double cum = 0;
        for (int i = 0; i < (int)duration; i++)
            cum += evaporation[i];
        return cum;
    }

    public double GetcumulativeTranspiration()
    {
        double cum = 0;
        for (int i = 0; i < (int)duration; i++)
            cum += transpire[i];
        return cum;
    }

    public double GetAverageDroughtIndexPlant()
    {
        double averageDroughtIndex = 0;
        for (int i = 0; i < duration; i++)
            averageDroughtIndex += droughtFactorPlant[i];
        averageDroughtIndex /= duration;
        return averageDroughtIndex;
    }

    public void CalculateNinputs(double leachingFrac, ref double unaccountedN)
    {
        CalculateManureInput(false);
        CalculateFertiliserInput();
        double totalNinput = totalManureNApplied + fertiliserNinput + NDepositionRate + Nfixed;
        if ((name.CompareTo("Grass") == 0) && ((totalManureNApplied + fertiliserNinput) == 0))
            F_CR = 0;
        else
            CalculateCropResidues();
        soilN2OEmissionFactor = GlobalVars.Instance.theZoneData.getsoilN2OEmissionFactor();
        soilN2OEmission = NDepositionRate * soilN2OEmissionFactor;
        CropResidueN2OEmission = F_CR * soilN2OEmissionFactor;
        N2ONemission = fertiliserN2OEmission + manureN2OEmission + soilN2OEmission + CropResidueN2OEmission;
        N2Nemission = N2ONemission * soilN2Factor;
        double gaseousNloss = fertiliserNH3emission + manureNH3emission + N2ONemission + N2Nemission;
        double Nyield = 0;
        for (int i = 0; i < theProducts.Count; i++)
        {
            Nyield += theProducts[i].GetPotential_yield() * theProducts[i].composition.GetN_conc() ;
        }
        double Nsurplus = 0;
        Nsurplus = totalNinput - Nyield;
        if (Nyield<totalNinput)
        {
            double Nleaching = Nsurplus*leachingFrac;
            SetnitrateLeaching(Nleaching);
            unaccountedN = Nsurplus - Nleaching;
        }
        else
        {
            SetnitrateLeaching(0);
            unaccountedN = Nsurplus;
        }

    }

    public void WriteCropGHGbudget(double Nunaccounted)
    {
        double N2OCO2eq = N2ONemission * GlobalVars.Instance.GetCO2EqN2O();
        double soilN2OEmissionCO2eq = soilN2OEmission * GlobalVars.Instance.GetCO2EqN2O();
        double fertiliserN2OEmissionCO2eq = fertiliserN2OEmission * GlobalVars.Instance.GetCO2EqN2O();
        double manureN2OEmissionCO2eq = manureN2OEmission * GlobalVars.Instance.GetCO2EqN2O();
        double manureNH3emissionCO2eq = manureNH3emission * GlobalVars.Instance.GetIndirectNH3N2OFactor() * GlobalVars.Instance.GetCO2EqN2O();
        double fertNH3emissionCO2eq = fertiliserNH3emission * GlobalVars.Instance.GetIndirectNH3N2OFactor() * GlobalVars.Instance.GetCO2EqN2O();
        double cropResidueN2OEmissionCO2eq= CropResidueN2OEmission * GlobalVars.Instance.GetCO2EqN2O();
        double leachedNCO2Eq = GetnitrateLeaching() * GlobalVars.Instance.GetIndirectNO3N2OFactor() * GlobalVars.Instance.GetCO2EqN2O();
        double totalNH3emission = manureNH3emission + fertiliserNH3emission;

        VMP3.Instance.WriteField(Convert.ToString(N2OCO2eq) + "\t" + Convert.ToString(soilN2OEmissionCO2eq) + "\t" + Convert.ToString(fertiliserN2OEmissionCO2eq) + 
            "\t" + Convert.ToString(manureN2OEmissionCO2eq) + "\t" + Convert.ToString(cropResidueN2OEmissionCO2eq) + "\t"+ Convert.ToString(fertNH3emissionCO2eq) + "\t" + 
            Convert.ToString(manureNH3emissionCO2eq) + "\t" + Convert.ToString(leachedNCO2Eq) + "\t" + 
            Convert.ToString(nitrateLeaching) + "\t" + Convert.ToString(totalNH3emission) + "\t" + Convert.ToString(Nunaccounted) + "\t");
        int manType;
        int speciesGroup;
        double amountN;

        VMP3.Instance.WriteField(name + "\t");
        if (fertiliserApplied.Count == 0)
            VMP3.Instance.WriteField('0' + "\t");
        else
        {
            for (int i = 0; i < fertiliserApplied.Count; i++)
            {
                amountN = fertiliserApplied[i].getNamount();
                VMP3.Instance.WriteField(Convert.ToString(amountN) + "\t");
            }
        }
        for (int i = 0; i < manureApplied.Count; i++)
        {
            manType = manureApplied[i].getManureType();
            speciesGroup = manureApplied[i].getspeciesGroup();
            amountN = manureApplied[i].getNamount();
//            VMP3.Instance.WriteField(Convert.ToString(speciesGroup) + "\t" + Convert.ToString(manType) + "\t" + Convert.ToString(amountN) + "\t");
            VMP3.Instance.WriteField(Convert.ToString(manType) + "\t" + Convert.ToString(amountN) + "\t");
        }
    }
}
