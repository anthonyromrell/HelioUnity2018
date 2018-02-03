//Maya ASCII 2018 scene
//Name: dh.ma
//Last modified: Thu, Feb 01, 2018 09:27:40 PM
//Codeset: 1252
requires maya "2018";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2018";
fileInfo "version" "2018";
fileInfo "cutIdentifier" "201706261615-f9658c4cfc";
fileInfo "osv" "Microsoft Windows 8 Home Premium Edition, 64-bit  (Build 9200)\n";
fileInfo "license" "student";
createNode transform -n "pCube1";
	rename -uid "8F928592-426C-053C-225A-9B9CE2DADBDE";
	setAttr ".t" -type "double3" 1.14903620821757 2.4036295649044574 1.1482448796142855 ;
	setAttr ".s" -type "double3" 1 2.3217275846862737 1 ;
createNode mesh -n "pCubeShape1" -p "pCube1";
	rename -uid "AF3FC81E-4675-E155-001F-50B0DB975076";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.54122253122723052 0.8976200005385726 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 4 ".pt[16:19]" -type "float3"  1.1920929e-07 0 0 1.1920929e-07 
		0 0 1.1920929e-07 0 0 1.1920929e-07 0 0;
	setAttr ".ai_translator" -type "string" "polymesh";
createNode mesh -n "polySurfaceShape1" -p "pCube1";
	rename -uid "2631FD63-49EB-D103-7448-CC93CD78ABB7";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.5 0.5 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 34 ".uvst[0].uvsp[0:33]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25 0.375 0.25 0.625 0.25 0.625 0.5 0.375 0.5 0.625 0.25
		 0.625 0.5 0.625 0.5 0.625 0.25 0.375 0.5 0.375 0.25 0.375 0.25 0.375 0.5 0 0 1 0
		 1 1 0 1 0.375 0.5 0.625 0.5 0.625 0.75 0.375 0.75;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 4 ".pt";
	setAttr ".pt[4]" -type "float3" 0 0 0.091316916 ;
	setAttr ".pt[5]" -type "float3" 0 0 0.091316916 ;
	setAttr ".pt[22]" -type "float3" 0 0 0.091316916 ;
	setAttr ".pt[23]" -type "float3" 0 0 0.091316916 ;
	setAttr -s 26 ".vt[0:25]"  -0.68487245 -0.5 0.5 0.68487239 -0.49999976 0.49999976
		 -1.038029671 0.49999988 0.75782692 1.038029671 0.49999964 0.75782657 -1.038029671 0.49999988 -0.75782692
		 1.038029671 0.49999964 -0.75782657 -0.68487245 -0.5 -0.5 0.68487239 -0.49999976 -0.49999976
		 0.023928642 1.11335909 0.78443682 0.020321012 1.11335909 0.78443646 0.020321012 1.11335909 -0.7844364
		 0.026195407 1.11317265 -0.75395501 1.48338294 0.43780935 1.083776474 1.48338294 0.43780935 -1.083776355
		 0.027946353 1.31498182 -1.12183142 0.027946353 1.31498182 1.12183142 -1.46472907 0.41545522 1.11094975
		 -1.46472907 0.41545522 -1.11094999 0.02720058 1.3146199 1.14995861 0.029467344 1.31443346 -1.11947691
		 0.021675348 1.11335909 -0.7844364 0.030235052 1.31278646 -1.11621535 -0.55108553 0.016410708 -0.85357916
		 0.55108553 0.1857388 -0.85357898 0.36359569 -0.49167025 -0.71669984 -0.36359569 -0.49167028 -0.71669996;
	setAttr -s 47 ".ed[0:46]"  0 1 0 2 3 1 4 5 0 6 7 1 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 1 5 7 1 6 0 0 7 1 0 2 8 1 3 9 1 8 9 0 5 10 1 9 10 0 4 11 1 11 10 0 8 11 0
		 3 12 0 5 13 0 12 13 0 10 14 0 13 14 0 9 15 0 15 14 0 12 15 0 2 16 0 4 17 0 16 17 0
		 8 18 0 16 18 0 11 19 0 18 19 0 17 19 0 10 20 0 14 21 0 20 21 0 4 22 0 5 23 0 22 23 0
		 7 24 0 23 24 0 6 25 0 25 24 0 22 25 0;
	setAttr -s 22 -ch 88 ".fc[0:21]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 14 16 -19 -20
		mu 0 4 14 15 16 17
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13
		f 4 1 13 -15 -13
		mu 0 4 2 3 15 14
		f 4 22 24 -27 -28
		mu 0 4 18 19 20 21
		f 4 -3 17 18 -16
		mu 0 4 5 4 17 16
		f 4 -31 32 34 -36
		mu 0 4 22 23 24 25
		f 4 7 21 -23 -21
		mu 0 4 3 5 19 18
		f 4 15 23 -25 -22
		mu 0 4 5 16 20 19
		f 4 -17 25 26 -24
		mu 0 4 16 15 21 20
		f 4 -14 20 27 -26
		mu 0 4 15 3 18 21
		f 4 -7 28 30 -30
		mu 0 4 4 2 23 22
		f 4 12 31 -33 -29
		mu 0 4 2 14 24 23
		f 4 19 33 -35 -32
		mu 0 4 14 17 25 24
		f 4 -18 29 35 -34
		mu 0 4 17 4 22 25
		f 4 23 37 -39 -37
		mu 0 4 26 27 28 29
		f 4 2 40 -42 -40
		mu 0 4 4 5 31 30
		f 4 9 42 -44 -41
		mu 0 4 5 7 32 31
		f 4 -4 44 45 -43
		mu 0 4 7 6 33 32
		f 4 -9 39 46 -45
		mu 0 4 6 4 30 33;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".ai_translator" -type "string" "polymesh";
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 22 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surface" "Particles" "Particle Instance" "Fluids" "Strokes" "Image Planes" "UI" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Hair Systems" "Follicles" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 22 0 1 1 1 1 1
		 1 1 1 0 0 0 0 0 0 0 0 0
		 0 0 0 0 ;
	setAttr ".fprt" yes;
select -ne :renderPartition;
	setAttr -s 2 ".st";
select -ne :renderGlobalsList1;
select -ne :defaultShaderList1;
	setAttr -s 4 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderingList1;
select -ne :initialShadingGroup;
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultRenderGlobals;
	setAttr ".ren" -type "string" "arnold";
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
connectAttr "polySurfaceShape1.o" "pCubeShape1.i";
connectAttr "pCubeShape1.iog" ":initialShadingGroup.dsm" -na;
// End of dh.ma
