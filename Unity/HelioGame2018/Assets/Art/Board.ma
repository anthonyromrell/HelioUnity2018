//Maya ASCII 2017ff05 scene
//Name: Board.ma
//Last modified: Fri, Feb 16, 2018 04:40:53 PM
//Codeset: UTF-8
requires maya "2017ff05";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2017";
fileInfo "version" "2017";
fileInfo "cutIdentifier" "201706020738-1017329";
fileInfo "osv" "Mac OS X 10.13.3";
createNode transform -s -n "persp";
	rename -uid "F6D3EDAC-234B-3962-BBB5-E788E49A5486";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 192.78161895923719 123.54905888194004 -38.871592946439939 ;
	setAttr ".r" -type "double3" -32.138352729788842 101.39999999987214 0 ;
	setAttr ".rp" -type "double3" 5.3290705182007514e-15 2.19824158875781e-14 0 ;
	setAttr ".rpt" -type "double3" -1.5788761570428148e-14 -9.8843467092545828e-15 -1.5575858326750961e-14 ;
createNode camera -s -n "perspShape" -p "persp";
	rename -uid "C4139C57-C447-4FD1-BCAA-A8B09D3F1CBC";
	setAttr -k off ".v" no;
	setAttr ".fl" 34.999999999999986;
	setAttr ".coi" 232.25013088770035;
	setAttr ".imn" -type "string" "persp";
	setAttr ".den" -type "string" "persp_depth";
	setAttr ".man" -type "string" "persp_mask";
	setAttr ".hc" -type "string" "viewSet -p %camera";
createNode transform -s -n "top";
	rename -uid "38F75E8F-DD44-BA92-CFE7-C1BC8038E351";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 0 1000.1 0 ;
	setAttr ".r" -type "double3" -89.999999999999986 0 0 ;
createNode camera -s -n "topShape" -p "top";
	rename -uid "8623CEBB-A843-0040-5541-4BA1CA4F4C63";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 1000.1;
	setAttr ".ow" 30;
	setAttr ".imn" -type "string" "top";
	setAttr ".den" -type "string" "top_depth";
	setAttr ".man" -type "string" "top_mask";
	setAttr ".hc" -type "string" "viewSet -t %camera";
	setAttr ".o" yes;
createNode transform -s -n "front";
	rename -uid "45254085-044E-35B3-AD43-F28B591BC718";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 0 0.049999997019765985 1000.2511455541681 ;
createNode camera -s -n "frontShape" -p "front";
	rename -uid "A2040ED0-CD48-9843-9460-0D817267ED09";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 1000.2511455541681;
	setAttr ".ow" 3.7388222822404575;
	setAttr ".imn" -type "string" "front";
	setAttr ".den" -type "string" "front_depth";
	setAttr ".man" -type "string" "front_mask";
	setAttr ".tp" -type "double3" 0 0.049999997019765985 0 ;
	setAttr ".hc" -type "string" "viewSet -f %camera";
	setAttr ".o" yes;
createNode transform -s -n "side";
	rename -uid "D18BBE1D-5347-4653-9E1E-F1AA8D036FF7";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 1000.1 0 0 ;
	setAttr ".r" -type "double3" 0 89.999999999999986 0 ;
createNode camera -s -n "sideShape" -p "side";
	rename -uid "BBD13F3D-F04E-DBD0-0EE6-DBBAC2D155E2";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 1000.1;
	setAttr ".ow" 30;
	setAttr ".imn" -type "string" "side";
	setAttr ".den" -type "string" "side_depth";
	setAttr ".man" -type "string" "side_mask";
	setAttr ".hc" -type "string" "viewSet -s %camera";
	setAttr ".o" yes;
createNode transform -n "group1";
	rename -uid "9967B288-3844-B053-6DB9-6BA467612554";
	setAttr ".t" -type "double3" 0 0 -24 ;
	setAttr ".rp" -type "double3" 1.5162284611609991 0.099999904632568359 1.5182990337824696 ;
	setAttr ".sp" -type "double3" 1.5162284611609991 0.099999904632568359 1.5182990337824696 ;
createNode transform -n "group4";
	rename -uid "12A5BB79-A14E-92B2-7BFE-65A89412FC69";
	setAttr ".t" -type "double3" 6 0 -48 ;
	setAttr ".rp" -type "double3" 1.4999999999999964 0.099999994039535522 1.5000000000000018 ;
	setAttr ".sp" -type "double3" 1.4999999999999964 0.099999994039535522 1.5000000000000018 ;
createNode transform -n "group3" -p "group4";
	rename -uid "EC463516-6148-1057-FC9C-DA8DA61B1830";
	setAttr ".t" -type "double3" 1.3322676295501878e-15 0 3 ;
	setAttr ".r" -type "double3" 0 -89.999999999999972 0 ;
	setAttr ".rp" -type "double3" 0 0.099999994039535522 0 ;
	setAttr ".sp" -type "double3" 0 0.099999994039535522 0 ;
createNode transform -n "group2" -p "group4";
	rename -uid "B01B58A4-4245-248B-5EB7-7B8D2E718C90";
	setAttr ".rp" -type "double3" 0 0.099999994039535522 0 ;
	setAttr ".sp" -type "double3" 0 0.099999994039535522 0 ;
createNode transform -n "group5" -p "group4";
	rename -uid "BF4EA27F-7840-754C-4698-C497BA8C0555";
	setAttr ".t" -type "double3" 3 0 2.4424906541753448e-15 ;
	setAttr ".r" -type "double3" 0 -89.999999999999986 0 ;
	setAttr ".rp" -type "double3" 0 0.099999994039535522 0 ;
	setAttr ".sp" -type "double3" 0 0.099999994039535522 0 ;
createNode transform -n "group6" -p "group4";
	rename -uid "886A7091-3547-9C75-9AF7-7EBD6276D4B7";
	setAttr ".t" -type "double3" 3.0000000000000053 0 3.0000000000000027 ;
	setAttr ".r" -type "double3" 0 -179.99999999999991 0 ;
	setAttr ".rp" -type "double3" 0 0.099999994039535522 0 ;
	setAttr ".sp" -type "double3" 0 0.099999994039535522 0 ;
createNode transform -n "group7" -p "group4";
	rename -uid "C0432201-5445-5F91-1064-629FF0F1FAB1";
	setAttr ".t" -type "double3" 5.329070518200749e-15 0 3.0000000000000062 ;
	setAttr ".r" -type "double3" 0 -270.00000000000006 0 ;
	setAttr ".rp" -type "double3" 0 0.099999994039535522 0 ;
	setAttr ".sp" -type "double3" 0 0.099999994039535522 0 ;
createNode transform -n "group10";
	rename -uid "7D720C9E-4049-66F7-DC43-B3972536D801";
	setAttr ".t" -type "double3" 0 0 30.5 ;
	setAttr ".rp" -type "double3" 10.516228461160994 0.099999904632568359 10.536598067564933 ;
	setAttr ".sp" -type "double3" 10.516228461160994 0.099999904632568359 10.536598067564933 ;
createNode transform -n "group8" -p "group10";
	rename -uid "E34A079C-3348-CB2E-F863-1794CC6A2E02";
	setAttr ".rp" -type "double3" 10.516228461160994 0.099999994039535522 10.536598067564933 ;
	setAttr ".sp" -type "double3" 10.516228461160994 0.099999994039535522 10.536598067564933 ;
createNode transform -n "group9" -p "group10";
	rename -uid "EC600F1C-0444-9301-993A-E495F95DD19D";
	setAttr ".r" -type "double3" 0 89.999999999999972 0 ;
	setAttr ".rp" -type "double3" 10.516228461160994 0.099999994039535522 10.536598067564933 ;
	setAttr ".sp" -type "double3" 10.516228461160994 0.099999994039535522 10.536598067564933 ;
createNode transform -n "ChessHitGrid";
	rename -uid "B599B002-E442-781D-7535-5686B451EB7E";
	setAttr ".t" -type "double3" 0 0 -40 ;
createNode transform -n "HitBox4" -p "ChessHitGrid";
	rename -uid "86E261A8-4D41-A50C-40E0-77937B0041AE";
	setAttr ".t" -type "double3" -14 2 2 ;
createNode mesh -n "HitBox4Shape" -p "HitBox4";
	rename -uid "6DFBDEEE-3348-B6C4-BAC2-84B968C8A2C0";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox5" -p "ChessHitGrid";
	rename -uid "98995E49-9F44-20D7-BB12-D0BD0D6ED522";
	setAttr ".t" -type "double3" -14 2 6 ;
createNode mesh -n "HitBox5Shape" -p "HitBox5";
	rename -uid "6DCF0585-1949-DC20-86D4-0DAACFD6A5F4";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox" -p "ChessHitGrid";
	rename -uid "A22E882B-6540-FB4F-BA70-729EDA07B30C";
	setAttr ".t" -type "double3" -14 2 -14 ;
createNode mesh -n "HitBoxShape" -p "|ChessHitGrid|HitBox";
	rename -uid "9DB6F556-7D43-B9AC-D5AE-169C2BB39109";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode transform -n "HitBox7" -p "ChessHitGrid";
	rename -uid "9F0067F1-844E-A9F5-00CD-D0961615B08C";
	setAttr ".t" -type "double3" -14 2 14 ;
createNode mesh -n "HitBox7Shape" -p "|ChessHitGrid|HitBox7";
	rename -uid "A3F1047F-D448-EF7E-236F-A78DF63BAC62";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox6" -p "ChessHitGrid";
	rename -uid "EBA9BAC3-104E-35DC-E669-98A3C5F2FCFD";
	setAttr ".t" -type "double3" -14 2 10 ;
createNode mesh -n "HitBox6Shape" -p "HitBox6";
	rename -uid "1C58AD15-8547-F603-2744-359231848ACF";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox33" -p "ChessHitGrid";
	rename -uid "81B49961-BC4C-34CD-29C0-EF869D2A77BD";
	setAttr ".t" -type "double3" 2 2 6 ;
createNode mesh -n "HitBox33Shape" -p "HitBox33";
	rename -uid "438AF66F-5C43-52B4-607C-BA9A1C69BE13";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox34" -p "ChessHitGrid";
	rename -uid "B9A801FC-B142-E132-D0AE-AE98F81AD82B";
	setAttr ".t" -type "double3" 2 2 -14 ;
createNode mesh -n "HitBox34Shape" -p "HitBox34";
	rename -uid "ED3BEA38-DB4A-3D43-E679-56B7EBD0FF4B";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox35" -p "ChessHitGrid";
	rename -uid "A40CC3DE-0048-E64A-451D-3886E239D527";
	setAttr ".t" -type "double3" 2 2 14 ;
createNode mesh -n "HitBox35Shape" -p "|ChessHitGrid|HitBox35";
	rename -uid "01512283-1346-A28E-DB5A-8EB7BB617CEF";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox37" -p "ChessHitGrid";
	rename -uid "50986738-5E47-9879-00EB-6F8697C90B7F";
	setAttr ".t" -type "double3" 2 2 -6 ;
createNode mesh -n "HitBox37Shape" -p "HitBox37";
	rename -uid "171EECEF-9746-EDB5-1D93-F0A0A86CA543";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox36" -p "ChessHitGrid";
	rename -uid "2504BD46-F543-D243-058C-03BE510C61B0";
	setAttr ".t" -type "double3" 2 2 10 ;
createNode mesh -n "HitBox36Shape" -p "HitBox36";
	rename -uid "CC65F305-0342-766F-2E94-58B481A23B33";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox32" -p "ChessHitGrid";
	rename -uid "AF1C1B33-4B43-AB1F-3233-CAB398BE2344";
	setAttr ".t" -type "double3" 2 2 2 ;
createNode mesh -n "HitBox32Shape" -p "HitBox32";
	rename -uid "B840C94B-CD43-CBF5-A859-94AF055F965A";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox63" -p "ChessHitGrid";
	rename -uid "33ED62F6-6949-FEBE-B477-AA90B3215D50";
	setAttr ".t" -type "double3" 14 2 -10 ;
createNode mesh -n "HitBox63Shape" -p "HitBox63";
	rename -uid "846ABF88-9B49-1BDE-6911-4D9BA42DDA01";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox62" -p "ChessHitGrid";
	rename -uid "2E03C6F4-B248-F456-0F6C-E1AD8077332D";
	setAttr ".t" -type "double3" 14 2 -2 ;
createNode mesh -n "HitBox62Shape" -p "HitBox62";
	rename -uid "442C8B76-524D-035A-8B13-6FA9C33A23B2";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox61" -p "ChessHitGrid";
	rename -uid "0DBA93D0-254F-796C-0833-13B50EB446C6";
	setAttr ".t" -type "double3" 14 2 -6 ;
createNode mesh -n "HitBox61Shape" -p "HitBox61";
	rename -uid "E8AE528E-434F-4A57-2949-E8AA9D9B2C06";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox60" -p "ChessHitGrid";
	rename -uid "FBC8F8C6-BF4D-C820-8A66-6D80E20FBC68";
	setAttr ".t" -type "double3" 14 2 10 ;
createNode mesh -n "HitBox60Shape" -p "|ChessHitGrid|HitBox60";
	rename -uid "7C40264F-3344-9A4B-5748-63AA22D3561F";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox59" -p "ChessHitGrid";
	rename -uid "8E56ABC8-F544-8768-7E00-388ABCACC52F";
	setAttr ".t" -type "double3" 14 2 14 ;
createNode mesh -n "HitBox59Shape" -p "HitBox59";
	rename -uid "E4DDD5A0-5E49-701E-CEB5-A6A2A30FE1CE";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox58" -p "ChessHitGrid";
	rename -uid "FA5C23B5-AB40-5996-0C25-D6808B57E5BB";
	setAttr ".t" -type "double3" 14 2 -14 ;
createNode mesh -n "HitBox58Shape" -p "HitBox58";
	rename -uid "44C28178-4849-06AB-8EEC-8CB1B2603FCB";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox57" -p "ChessHitGrid";
	rename -uid "B6922CA8-D945-D600-F8AB-46B70CF7B401";
	setAttr ".t" -type "double3" 14 2 6 ;
createNode mesh -n "HitBox57Shape" -p "|ChessHitGrid|HitBox57";
	rename -uid "2CBAE16D-4546-E5EA-75B8-5E8C63A8FE71";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox56" -p "ChessHitGrid";
	rename -uid "0F0A4DE6-7E4D-02EE-B5D6-07AE61B0A369";
	setAttr ".t" -type "double3" 14 2 2 ;
createNode mesh -n "HitBox56Shape" -p "|ChessHitGrid|HitBox56";
	rename -uid "865D47DB-2840-29CC-74B4-0590F8D3ECDF";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox55" -p "ChessHitGrid";
	rename -uid "BF2251F6-764E-094D-0C2B-AAA789A57F54";
	setAttr ".t" -type "double3" 10 2 -10 ;
createNode mesh -n "HitBox55Shape" -p "HitBox55";
	rename -uid "9E9B4B56-D249-2147-AC23-0285F3387E4B";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox54" -p "ChessHitGrid";
	rename -uid "37C6964F-AA48-92FA-FCB8-09936C3EDB1F";
	setAttr ".t" -type "double3" 10 2 -2 ;
createNode mesh -n "HitBox54Shape" -p "|ChessHitGrid|HitBox54";
	rename -uid "04F431F1-9E43-896B-C3F0-82B19D97370C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox53" -p "ChessHitGrid";
	rename -uid "1BE4D8BD-7341-3BDC-72EA-489EBE237477";
	setAttr ".t" -type "double3" 10 2 -6 ;
createNode mesh -n "HitBox53Shape" -p "HitBox53";
	rename -uid "957ADC21-D84E-96FD-85A8-39BA425F9DE6";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox52" -p "ChessHitGrid";
	rename -uid "7F3EC9EA-A44D-B5BC-2B41-1C9D5C6AA890";
	setAttr ".t" -type "double3" 10 2 10 ;
createNode mesh -n "HitBox52Shape" -p "HitBox52";
	rename -uid "39673836-2C46-CE69-3E81-1CB149AAA778";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox51" -p "ChessHitGrid";
	rename -uid "66C62C78-C74B-C068-46CA-1EA4A3C91CEA";
	setAttr ".t" -type "double3" 10 2 14 ;
createNode mesh -n "HitBox51Shape" -p "|ChessHitGrid|HitBox51";
	rename -uid "BA4713F4-E849-1988-E34A-EF809F96A50C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox50" -p "ChessHitGrid";
	rename -uid "57E876AB-A444-3964-B1CC-29BAE41A95DB";
	setAttr ".t" -type "double3" 10 2 -14 ;
createNode mesh -n "HitBox50Shape" -p "HitBox50";
	rename -uid "EA7369DB-144F-B8B2-35A6-9BAF42F84FDC";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox49" -p "ChessHitGrid";
	rename -uid "02C707F5-EE4A-B885-CFA5-539EBEAF7359";
	setAttr ".t" -type "double3" 10 2 6 ;
createNode mesh -n "HitBox49Shape" -p "HitBox49";
	rename -uid "E2E63590-0047-A67C-A1F8-7A9DBBAC9E75";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox48" -p "ChessHitGrid";
	rename -uid "00484A6A-EC4B-4DD3-A1CF-9A9B1B9468C5";
	setAttr ".t" -type "double3" 10 2 2 ;
createNode mesh -n "HitBox48Shape" -p "HitBox48";
	rename -uid "AFF3C7C2-ED4D-97EF-D15A-B982CF83A107";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox47" -p "ChessHitGrid";
	rename -uid "29C2FA24-E447-36F7-A847-CE87A343D7A9";
	setAttr ".t" -type "double3" 6 2 -10 ;
createNode mesh -n "HitBox47Shape" -p "HitBox47";
	rename -uid "F6917FF9-E843-C44A-14D2-2BAC21D3B759";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox46" -p "ChessHitGrid";
	rename -uid "99D3C7E2-F843-0522-7250-81A2BFF0AA91";
	setAttr ".t" -type "double3" 6 2 -2 ;
createNode mesh -n "HitBox46Shape" -p "HitBox46";
	rename -uid "C5D57369-F346-8927-C008-BC8445DBD561";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox45" -p "ChessHitGrid";
	rename -uid "B6F18436-604B-91F5-661C-ABAA3EE7CB9D";
	setAttr ".t" -type "double3" 6 2 -6 ;
createNode mesh -n "HitBox45Shape" -p "HitBox45";
	rename -uid "02F8BCCF-074C-1AC2-4114-9386E8D4F0AA";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox44" -p "ChessHitGrid";
	rename -uid "09B5C144-4A47-5C35-FC43-2A88CE09C940";
	setAttr ".t" -type "double3" 6 2 10 ;
createNode mesh -n "HitBox44Shape" -p "HitBox44";
	rename -uid "D4F9ACF8-C743-ADBE-E69A-C4BBA419C951";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox43" -p "ChessHitGrid";
	rename -uid "A13DD756-B940-E083-BF28-9ABE4579FAA6";
	setAttr ".t" -type "double3" 6 2 14 ;
createNode mesh -n "HitBox43Shape" -p "|ChessHitGrid|HitBox43";
	rename -uid "2E84AB84-124C-1C3F-3020-4591EED04372";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox42" -p "ChessHitGrid";
	rename -uid "A5C9A7BF-6A46-2369-5CA7-94868ECAEF97";
	setAttr ".t" -type "double3" 6 2 -14 ;
createNode mesh -n "HitBox42Shape" -p "HitBox42";
	rename -uid "545BC786-2D44-5DEB-968B-B88D290FC375";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox41" -p "ChessHitGrid";
	rename -uid "F797FEAC-9142-88B1-6FB6-84BE936A4FB6";
	setAttr ".t" -type "double3" 6 2 6 ;
createNode mesh -n "HitBox41Shape" -p "HitBox41";
	rename -uid "C3877090-9742-EA35-42D0-5996F7E5E0E2";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox40" -p "ChessHitGrid";
	rename -uid "39F6962A-2147-57B9-EB94-3F97D827FC3C";
	setAttr ".t" -type "double3" 6 2 2 ;
createNode mesh -n "HitBox40Shape" -p "HitBox40";
	rename -uid "22AB40AF-F340-D78F-4A55-9AA2F5DA0AB9";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox21" -p "ChessHitGrid";
	rename -uid "2C3A1E95-0E4C-7B82-C634-38BBF4D53975";
	setAttr ".t" -type "double3" -6 2 -6 ;
createNode mesh -n "HitBox21Shape" -p "HitBox21";
	rename -uid "84377A53-DF49-0E36-0BFD-759E4219349A";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox20" -p "ChessHitGrid";
	rename -uid "5D9EECA5-4140-C86E-57C2-F0A1D67CE2D1";
	setAttr ".t" -type "double3" -6 2 10 ;
createNode mesh -n "HitBox20Shape" -p "HitBox20";
	rename -uid "AF574C7A-2A46-1B7C-ED66-BEB47999EF98";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox23" -p "ChessHitGrid";
	rename -uid "65DB14B7-5546-B986-37C4-9DA90D53EA3D";
	setAttr ".t" -type "double3" -6 2 -10 ;
createNode mesh -n "HitBox23Shape" -p "HitBox23";
	rename -uid "37874198-5E42-9600-AF34-15BB882406AC";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox19" -p "ChessHitGrid";
	rename -uid "9552DD69-954E-18E6-6548-F38DA6BB8590";
	setAttr ".t" -type "double3" -6 2 14 ;
createNode mesh -n "HitBox19Shape" -p "|ChessHitGrid|HitBox19";
	rename -uid "D4196C5A-BB4D-CB55-0AD7-25A1E1C85C57";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox22" -p "ChessHitGrid";
	rename -uid "9BDBEB30-B94A-104A-2E00-E790555FE414";
	setAttr ".t" -type "double3" -6 2 -2 ;
createNode mesh -n "HitBox22Shape" -p "HitBox22";
	rename -uid "A784A22E-D446-7BB8-B348-319779460F44";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox18" -p "ChessHitGrid";
	rename -uid "C2C275B4-0A43-3EF7-0109-4791FB4FC861";
	setAttr ".t" -type "double3" -6 2 -14 ;
createNode mesh -n "HitBox18Shape" -p "HitBox18";
	rename -uid "76730802-2C43-C818-5F72-768A0C60D8C5";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox28" -p "ChessHitGrid";
	rename -uid "8BFEBBA6-B14D-003F-B0DD-31A639A6C48A";
	setAttr ".t" -type "double3" -2 2 10 ;
createNode mesh -n "HitBox28Shape" -p "|ChessHitGrid|HitBox28";
	rename -uid "97828B62-8343-4CAB-BB39-A28A60AAF4EA";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox31" -p "ChessHitGrid";
	rename -uid "08B79BBD-D24F-76A7-0FBA-97A6E0066279";
	setAttr ".t" -type "double3" -2 2 -10 ;
createNode mesh -n "HitBox31Shape" -p "HitBox31";
	rename -uid "B3567C17-7D41-2AF3-943C-E4A95FA874DE";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox29" -p "ChessHitGrid";
	rename -uid "2084E04A-9C47-2A54-131B-FBBDD9AACFA2";
	setAttr ".t" -type "double3" -2 2 -6 ;
createNode mesh -n "HitBox29Shape" -p "HitBox29";
	rename -uid "A4F762CA-EA49-317C-1D6F-C99DB46D7A43";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox30" -p "ChessHitGrid";
	rename -uid "9C39CA77-CD4C-EDBD-9145-74B5D615220A";
	setAttr ".t" -type "double3" -2 2 -2 ;
createNode mesh -n "HitBox30Shape" -p "HitBox30";
	rename -uid "7A0F1A96-A742-5832-8F14-B783B3AA06D7";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox27" -p "ChessHitGrid";
	rename -uid "DE44C191-D44C-D0FA-1572-FAB04A1D8707";
	setAttr ".t" -type "double3" -2 2 14 ;
createNode mesh -n "HitBox27Shape" -p "|ChessHitGrid|HitBox27";
	rename -uid "11304DD6-A341-99FC-5C04-BBAAB25A5B27";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox26" -p "ChessHitGrid";
	rename -uid "7F99B7D4-B54C-93B2-FC38-5BB788878DAE";
	setAttr ".t" -type "double3" -2 2 -14 ;
createNode mesh -n "HitBox26Shape" -p "HitBox26";
	rename -uid "579A97A2-6B4E-F1D0-0396-EEB1CBE528A5";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox25" -p "ChessHitGrid";
	rename -uid "60F28045-2944-8A5A-F247-5FA1B959E1B5";
	setAttr ".t" -type "double3" -2 2 6 ;
createNode mesh -n "HitBox25Shape" -p "HitBox25";
	rename -uid "728D2476-E745-F2F7-53C4-3EA3431D1AE7";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox24" -p "ChessHitGrid";
	rename -uid "75DE1B56-444B-3064-F295-EF85A55790CB";
	setAttr ".t" -type "double3" -2 2 2 ;
createNode mesh -n "HitBox24Shape" -p "|ChessHitGrid|HitBox24";
	rename -uid "9730C849-DE47-C2E3-C1EC-FCA35F31E94F";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox10" -p "ChessHitGrid";
	rename -uid "DE7C38CE-5940-5F1D-68CB-068C54A47C3F";
	setAttr ".t" -type "double3" -10 2 -14 ;
createNode mesh -n "HitBox10Shape" -p "HitBox10";
	rename -uid "33AB5572-A54C-8388-2F24-708426483AE8";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox9" -p "ChessHitGrid";
	rename -uid "A8115BFF-FF46-6AD2-0803-3FA46BE0BCE4";
	setAttr ".t" -type "double3" -10 2 6 ;
createNode mesh -n "HitBox9Shape" -p "HitBox9";
	rename -uid "D78F4142-C646-2E2D-CC19-7A8A9680065B";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox11" -p "ChessHitGrid";
	rename -uid "9CCD9FEB-5347-0053-7753-FB8307F93951";
	setAttr ".t" -type "double3" -10 2 14 ;
createNode mesh -n "HitBox11Shape" -p "|ChessHitGrid|HitBox11";
	rename -uid "7CB33944-B54D-396C-7C82-62A65AE67A8E";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox15" -p "ChessHitGrid";
	rename -uid "30A7EB88-5144-9C1D-1DA2-C1B617F22F93";
	setAttr ".t" -type "double3" -10 2 -10 ;
createNode mesh -n "HitBox15Shape" -p "HitBox15";
	rename -uid "8FF75795-EA4B-D2FE-7313-DBB59F6136B3";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox13" -p "ChessHitGrid";
	rename -uid "31612DD0-CF4A-8258-0948-B3A186C10FEF";
	setAttr ".t" -type "double3" -10 2 -6 ;
createNode mesh -n "HitBox13Shape" -p "HitBox13";
	rename -uid "C25933A9-4A41-06F6-8D25-6DB7EE6E0A3C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox12" -p "ChessHitGrid";
	rename -uid "EE6F01A2-FF46-4B48-38E0-FCB16CF11285";
	setAttr ".t" -type "double3" -10 2 10 ;
createNode mesh -n "HitBox12Shape" -p "HitBox12";
	rename -uid "B89569E9-304E-2A42-0C64-79B91248F199";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox8" -p "ChessHitGrid";
	rename -uid "0B633902-B34C-140E-506B-DB859C7667FF";
	setAttr ".t" -type "double3" -10 2 2 ;
createNode mesh -n "HitBox8Shape" -p "HitBox8";
	rename -uid "5E5FEC47-6D4B-93EF-93E6-6AB9E7B01E06";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox14" -p "ChessHitGrid";
	rename -uid "483CAE58-7544-DF38-45E1-4D8E3CDF5288";
	setAttr ".t" -type "double3" -10 2 -2 ;
createNode mesh -n "HitBox14Shape" -p "HitBox14";
	rename -uid "A696FA4E-7447-7665-8D0F-84B63800AC49";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox2" -p "ChessHitGrid";
	rename -uid "61F838DF-B040-3CAB-6F16-9DB5E74D993A";
	setAttr ".t" -type "double3" -14 2 -6 ;
createNode mesh -n "HitBox2Shape" -p "HitBox2";
	rename -uid "778997AD-A14A-41E1-339D-ECB07E8352BA";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox3" -p "ChessHitGrid";
	rename -uid "8771B5DA-304E-F27E-293A-B38C50F593F8";
	setAttr ".t" -type "double3" -14 2 -2 ;
createNode mesh -n "HitBox3Shape" -p "HitBox3";
	rename -uid "061E3A24-1B41-9181-B0B4-2DA2CB9FC36F";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox1" -p "ChessHitGrid";
	rename -uid "0593F594-1C48-41EE-99FA-91A8A1AEFD6F";
	setAttr ".t" -type "double3" -14 2 -10 ;
createNode mesh -n "HitBox1Shape" -p "HitBox1";
	rename -uid "B57A695B-4244-5B39-DC87-46994CF08F91";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox17" -p "ChessHitGrid";
	rename -uid "740E0ABA-8642-5EB7-D6FE-B68D8966EA1C";
	setAttr ".t" -type "double3" -6 2 6 ;
createNode mesh -n "HitBox17Shape" -p "HitBox17";
	rename -uid "40C74847-D243-12A0-7A2F-B0AC644F77ED";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox16" -p "ChessHitGrid";
	rename -uid "4DBEDD59-F34E-4478-5C98-22BDEB4EA74F";
	setAttr ".t" -type "double3" -6 2 2 ;
createNode mesh -n "HitBox16Shape" -p "HitBox16";
	rename -uid "994D7CFC-6E48-A345-F343-149DA009D454";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox39" -p "ChessHitGrid";
	rename -uid "F4EC44D3-5142-EB90-33DB-389E2644F1D0";
	setAttr ".t" -type "double3" 2 2 -10 ;
createNode mesh -n "HitBox39Shape" -p "HitBox39";
	rename -uid "F29D31E6-2E4C-439F-5217-FC9425EBB664";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox38" -p "ChessHitGrid";
	rename -uid "0794BE07-EC4E-FC58-EF45-7989202074BD";
	setAttr ".t" -type "double3" 2 2 -2 ;
createNode mesh -n "HitBox38Shape" -p "|ChessHitGrid|HitBox38";
	rename -uid "480EEB5A-6140-ACD6-70DD-D6A70A596F5E";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "Queen";
	rename -uid "E6E94E0B-564C-1E4B-3623-4CB3E4860095";
	setAttr ".t" -type "double3" -116 0 0 ;
createNode transform -n "HitBox51" -p "Queen";
	rename -uid "ACEEEAC3-4A4D-E9A8-B6E0-38B393D36018";
	setAttr ".rp" -type "double3" -4 2 0 ;
	setAttr ".sp" -type "double3" -4 2 0 ;
createNode mesh -n "HitBox51Shape" -p "|Queen|HitBox51";
	rename -uid "4863703B-CA4C-9E6C-2E49-53949DB7BE74";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 0 -4 2 0 -4 2 0 -4 2 
		0 -4 2 0 -4 2 0 -4 2 0 -4 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox43" -p "|Queen|HitBox51";
	rename -uid "4DF70B73-0A40-F43D-B3DC-EAA7E943D407";
	setAttr ".rp" -type "double3" -8 2 0 ;
	setAttr ".sp" -type "double3" -8 2 0 ;
createNode mesh -n "HitBox43Shape" -p "|Queen|HitBox51|HitBox43";
	rename -uid "A638F1B6-9041-5544-BDA6-4695D66423E1";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -8 2 0 -8 2 0 -8 2 0 -8 2 
		0 -8 2 0 -8 2 0 -8 2 0 -8 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox35" -p "|Queen|HitBox51|HitBox43";
	rename -uid "8A134ED5-E34D-A778-87A9-77B3AD83D9A1";
	setAttr ".rp" -type "double3" -12 2 0 ;
	setAttr ".sp" -type "double3" -12 2 0 ;
createNode mesh -n "HitBox35Shape" -p "|Queen|HitBox51|HitBox43|HitBox35";
	rename -uid "809C703C-A94E-4846-CFEC-A7A84121DDD8";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -12 2 0 -12 2 0 -12 2 0 -12 
		2 0 -12 2 0 -12 2 0 -12 2 0 -12 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox27" -p "|Queen|HitBox51|HitBox43|HitBox35";
	rename -uid "9C3BC019-104F-70E8-C4C9-22A94144D8BA";
	setAttr ".rp" -type "double3" -16 2 0 ;
	setAttr ".sp" -type "double3" -16 2 0 ;
createNode mesh -n "HitBox27Shape" -p "|Queen|HitBox51|HitBox43|HitBox35|HitBox27";
	rename -uid "A08BB0B7-094E-2D70-0B60-5CA2F1D6E0F4";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -16 2 0 -16 2 0 -16 2 0 -16 
		2 0 -16 2 0 -16 2 0 -16 2 0 -16 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox19" -p "|Queen|HitBox51|HitBox43|HitBox35|HitBox27";
	rename -uid "11A4AFD4-ED4E-5582-096B-B191924F3302";
	setAttr ".rp" -type "double3" -20 2 0 ;
	setAttr ".sp" -type "double3" -20 2 0 ;
createNode mesh -n "HitBox19Shape" -p "|Queen|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19";
	rename -uid "EF35A88B-6B4D-DFB4-B5A5-E1AB43F3D445";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -20 2 0 -20 2 0 -20 2 0 -20 
		2 0 -20 2 0 -20 2 0 -20 2 0 -20 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox11" -p "|Queen|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19";
	rename -uid "FF72E621-5847-9DBC-31D1-AA8A661D34B7";
	setAttr ".rp" -type "double3" -24 2 0 ;
	setAttr ".sp" -type "double3" -24 2 0 ;
createNode mesh -n "HitBox11Shape" -p "|Queen|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox11";
	rename -uid "D311185A-5C4D-513A-680B-AEA51663F69D";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -24 2 0 -24 2 0 -24 2 0 -24 
		2 0 -24 2 0 -24 2 0 -24 2 0 -24 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox7" -p "|Queen|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox11";
	rename -uid "AAA44B34-7F40-6D42-25CB-E3B55000969E";
	setAttr ".rp" -type "double3" -28 2 0 ;
	setAttr ".sp" -type "double3" -28 2 0 ;
createNode mesh -n "HitBox7Shape" -p "|Queen|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox11|HitBox7";
	rename -uid "9951E503-8F42-7DAE-F11D-12A3F0427E2F";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -28 2 0 -28 2 0 -28 2 0 -28 
		2 0 -28 2 0 -28 2 0 -28 2 0 -28 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox113" -p "Queen";
	rename -uid "74586CBC-9E4F-B01D-AF41-C98CCC19DA2F";
	setAttr ".rp" -type "double3" 4 2 0 ;
	setAttr ".sp" -type "double3" 4 2 0 ;
createNode mesh -n "HitBox113Shape" -p "|Queen|HitBox113";
	rename -uid "75CE0AD4-0A4F-4BD9-C5ED-8A819C85F5C1";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 0 4 2 0 4 2 0 4 2 0 4 
		2 0 4 2 0 4 2 0 4 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox113";
	rename -uid "FF46A17D-4A47-B8E9-9049-A990FF7C50D6";
	setAttr ".rp" -type "double3" 8 2 0 ;
	setAttr ".sp" -type "double3" 8 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox113|HitBox";
	rename -uid "7A1E6EE0-DF47-CDA8-8561-9880B341D961";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  8 2 0 8 2 0 8 2 0 8 2 0 8 
		2 0 8 2 0 8 2 0 8 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox113|HitBox";
	rename -uid "9D64714F-3E4E-DA84-8F30-E39A677727B7";
	setAttr ".rp" -type "double3" 12 2 0 ;
	setAttr ".sp" -type "double3" 12 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox113|HitBox|HitBox";
	rename -uid "8260E617-B24D-D43B-AB8A-F68185A0596C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  12 2 0 12 2 0 12 2 0 12 2 
		0 12 2 0 12 2 0 12 2 0 12 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox113|HitBox|HitBox";
	rename -uid "ABF35DCC-0A47-1BF4-5572-C48CEAE4E212";
	setAttr ".rp" -type "double3" 16 2 0 ;
	setAttr ".sp" -type "double3" 16 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox113|HitBox|HitBox|HitBox";
	rename -uid "170D8B81-8741-55D6-4BA7-808590E5C97E";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  16 2 0 16 2 0 16 2 0 16 2 
		0 16 2 0 16 2 0 16 2 0 16 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox113|HitBox|HitBox|HitBox";
	rename -uid "502E0E38-4147-4136-B154-929484C3BF4F";
	setAttr ".rp" -type "double3" 20 2 0 ;
	setAttr ".sp" -type "double3" 20 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox113|HitBox|HitBox|HitBox|HitBox";
	rename -uid "E2E4F8FB-8046-E135-A19D-629A0F2E7A05";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  20 2 0 20 2 0 20 2 0 20 2 
		0 20 2 0 20 2 0 20 2 0 20 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox113|HitBox|HitBox|HitBox|HitBox";
	rename -uid "65CD3535-7745-8C8D-C300-FFB75A29AB2D";
	setAttr ".rp" -type "double3" 24 2 0 ;
	setAttr ".sp" -type "double3" 24 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "5CAE3064-8748-DE85-B5A1-33AF9F486531";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  24 2 0 24 2 0 24 2 0 24 2 
		0 24 2 0 24 2 0 24 2 0 24 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "23FBB2E4-C04E-BBD8-01B4-B9B997A8EFD6";
	setAttr ".rp" -type "double3" 28 2 0 ;
	setAttr ".sp" -type "double3" 28 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "AF5E8D7F-D24C-4B51-DEDF-89999F1C5767";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  28 2 0 28 2 0 28 2 0 28 2 
		0 28 2 0 28 2 0 28 2 0 28 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox71" -p "Queen";
	rename -uid "2D8EDD68-CF4B-7653-9888-34B2C7A3A257";
	setAttr ".rp" -type "double3" 0 2 4 ;
	setAttr ".sp" -type "double3" 0 2 4 ;
createNode mesh -n "HitBox71Shape" -p "|Queen|HitBox71";
	rename -uid "1EC6D607-1249-7A88-A034-3DA545B22875";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 4 0 2 4 0 2 4 0 2 4 0 
		2 4 0 2 4 0 2 4 0 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox72" -p "|Queen|HitBox71";
	rename -uid "061A3E9A-B349-89D8-EEBB-6B94CCA9C64F";
	setAttr ".rp" -type "double3" 0 2 8 ;
	setAttr ".sp" -type "double3" 0 2 8 ;
createNode mesh -n "HitBox72Shape" -p "|Queen|HitBox71|HitBox72";
	rename -uid "7B95B2BE-3448-7953-AE19-EFB920010C60";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 8 0 2 8 0 2 8 0 2 8 0 
		2 8 0 2 8 0 2 8 0 2 8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox73" -p "|Queen|HitBox71|HitBox72";
	rename -uid "1AA8656E-2F49-F985-BB5A-D49941598320";
	setAttr ".rp" -type "double3" 0 2 12 ;
	setAttr ".sp" -type "double3" 0 2 12 ;
createNode mesh -n "HitBox73Shape" -p "|Queen|HitBox71|HitBox72|HitBox73";
	rename -uid "69975A86-8E4A-188C-C2D3-EF8A6E6D3CFB";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 12 0 2 12 0 2 12 0 2 
		12 0 2 12 0 2 12 0 2 12 0 2 12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox74" -p "|Queen|HitBox71|HitBox72|HitBox73";
	rename -uid "22F3E11C-CB4D-5739-E163-9484B4DFACBF";
	setAttr ".rp" -type "double3" 0 2 16 ;
	setAttr ".sp" -type "double3" 0 2 16 ;
createNode mesh -n "HitBox74Shape" -p "|Queen|HitBox71|HitBox72|HitBox73|HitBox74";
	rename -uid "9162A7D0-9F4B-2F70-7025-BA94E5DBEE14";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 16 0 2 16 0 2 16 0 2 
		16 0 2 16 0 2 16 0 2 16 0 2 16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox75" -p "|Queen|HitBox71|HitBox72|HitBox73|HitBox74";
	rename -uid "10356FA6-A847-A195-A1B4-89953929FFEB";
	setAttr ".rp" -type "double3" 0 2 20 ;
	setAttr ".sp" -type "double3" 0 2 20 ;
createNode mesh -n "HitBox75Shape" -p "|Queen|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75";
	rename -uid "05BC00EC-7144-888B-CBCC-43B0A4661FE1";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 20 0 2 20 0 2 20 0 2 
		20 0 2 20 0 2 20 0 2 20 0 2 20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox76" -p "|Queen|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75";
	rename -uid "4B3C8C73-464E-DE50-04C8-CCB7250EF439";
	setAttr ".rp" -type "double3" 0 2 24 ;
	setAttr ".sp" -type "double3" 0 2 24 ;
createNode mesh -n "HitBox76Shape" -p "|Queen|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox76";
	rename -uid "EDA7CFCD-7842-DB3B-3FFF-E0BD6F9742E1";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 24 0 2 24 0 2 24 0 2 
		24 0 2 24 0 2 24 0 2 24 0 2 24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox77" -p "|Queen|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox76";
	rename -uid "C31E1B36-4F44-D27E-196C-A68C80435430";
	setAttr ".rp" -type "double3" 0 2 28 ;
	setAttr ".sp" -type "double3" 0 2 28 ;
createNode mesh -n "HitBox77Shape" -p "|Queen|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox76|HitBox77";
	rename -uid "C06FCFED-2E49-EC24-7BD7-3094C543CD43";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 28 0 2 28 0 2 28 0 2 
		28 0 2 28 0 2 28 0 2 28 0 2 28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox78" -p "Queen";
	rename -uid "A779D904-7148-AA55-46D9-0D820B56A27C";
	setAttr ".rp" -type "double3" 4 2 4 ;
	setAttr ".sp" -type "double3" 4 2 4 ;
createNode mesh -n "HitBox78Shape" -p "|Queen|HitBox78";
	rename -uid "149DC1BE-074F-F5E2-E19D-54A53B636711";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 4 4 2 4 4 2 4 4 2 4 4 
		2 4 4 2 4 4 2 4 4 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox79" -p "|Queen|HitBox78";
	rename -uid "D5B40B0B-0145-746D-4C1B-CB82E485AFEB";
	setAttr ".rp" -type "double3" 8 2 8 ;
	setAttr ".sp" -type "double3" 8 2 8 ;
createNode mesh -n "HitBox79Shape" -p "|Queen|HitBox78|HitBox79";
	rename -uid "51B78768-C143-1CF3-3E42-44BF4559C1F8";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  8 2 8 8 2 8 8 2 8 8 2 8 8 
		2 8 8 2 8 8 2 8 8 2 8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox80" -p "|Queen|HitBox78|HitBox79";
	rename -uid "D767E843-9648-690D-F33D-DF98ECC1E05C";
	setAttr ".rp" -type "double3" 12 2 12 ;
	setAttr ".sp" -type "double3" 12 2 12 ;
createNode mesh -n "HitBox80Shape" -p "|Queen|HitBox78|HitBox79|HitBox80";
	rename -uid "0EFDF30B-4D42-A66A-5614-379F5B18A788";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  12 2 12 12 2 12 12 2 12 12 
		2 12 12 2 12 12 2 12 12 2 12 12 2 12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox81" -p "|Queen|HitBox78|HitBox79|HitBox80";
	rename -uid "A8AD5002-6E46-2221-CBC0-5FAEF92BC7A4";
	setAttr ".rp" -type "double3" 16 2 16 ;
	setAttr ".sp" -type "double3" 16 2 16 ;
createNode mesh -n "HitBox81Shape" -p "|Queen|HitBox78|HitBox79|HitBox80|HitBox81";
	rename -uid "893955C1-C74F-142A-1726-9DBBF764F6AC";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  16 2 16 16 2 16 16 2 16 16 
		2 16 16 2 16 16 2 16 16 2 16 16 2 16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox82" -p "|Queen|HitBox78|HitBox79|HitBox80|HitBox81";
	rename -uid "298991F4-9943-2D44-3B0A-FA9AA2DBA731";
	setAttr ".rp" -type "double3" 20 2 20 ;
	setAttr ".sp" -type "double3" 20 2 20 ;
createNode mesh -n "HitBox82Shape" -p "|Queen|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82";
	rename -uid "36223AF5-7946-BCCB-C878-3DBF21203168";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  20 2 20 20 2 20 20 2 20 20 
		2 20 20 2 20 20 2 20 20 2 20 20 2 20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox83" -p "|Queen|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82";
	rename -uid "214BCA24-A047-B928-8B6F-06BCDAB0A413";
	setAttr ".rp" -type "double3" 24 2 24 ;
	setAttr ".sp" -type "double3" 24 2 24 ;
createNode mesh -n "HitBox83Shape" -p "|Queen|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox83";
	rename -uid "D0AB6E02-6244-C5F8-E305-3A954647DC55";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  24 2 24 24 2 24 24 2 24 24 
		2 24 24 2 24 24 2 24 24 2 24 24 2 24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox84" -p "|Queen|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox83";
	rename -uid "AB360E21-B346-AB90-B047-74B7366B5717";
	setAttr ".rp" -type "double3" 28 2 28 ;
	setAttr ".sp" -type "double3" 28 2 28 ;
createNode mesh -n "HitBox84Shape" -p "|Queen|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox83|HitBox84";
	rename -uid "7511F0B7-974D-E516-187B-6983016BF00D";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  28 2 28 28 2 28 28 2 28 28 
		2 28 28 2 28 28 2 28 28 2 28 28 2 28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox85" -p "Queen";
	rename -uid "4290000E-5041-BD04-55A9-A98E69717144";
	setAttr ".rp" -type "double3" -4 2 4 ;
	setAttr ".sp" -type "double3" -4 2 4 ;
createNode mesh -n "HitBox85Shape" -p "|Queen|HitBox85";
	rename -uid "7D408B1F-8542-6B94-DC23-62A423BF847D";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 4 -4 2 4 -4 2 4 -4 2 
		4 -4 2 4 -4 2 4 -4 2 4 -4 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox86" -p "|Queen|HitBox85";
	rename -uid "8EB4EB91-A24F-767A-D2E5-FC9B2EBAC5EE";
	setAttr ".rp" -type "double3" -8 2 8 ;
	setAttr ".sp" -type "double3" -8 2 8 ;
createNode mesh -n "HitBox86Shape" -p "|Queen|HitBox85|HitBox86";
	rename -uid "5FA158E0-E640-389D-282D-CA88396A51E1";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -8 2 8 -8 2 8 -8 2 8 -8 2 
		8 -8 2 8 -8 2 8 -8 2 8 -8 2 8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox87" -p "|Queen|HitBox85|HitBox86";
	rename -uid "0ED2C45F-5248-89B3-5A3B-0EADBD8288A6";
	setAttr ".rp" -type "double3" -12 2 12 ;
	setAttr ".sp" -type "double3" -12 2 12 ;
createNode mesh -n "HitBox87Shape" -p "|Queen|HitBox85|HitBox86|HitBox87";
	rename -uid "DAEE5D5C-344D-45DF-4B0D-9D8CDCB6F590";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -12 2 12 -12 2 12 -12 2 12 
		-12 2 12 -12 2 12 -12 2 12 -12 2 12 -12 2 12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox88" -p "|Queen|HitBox85|HitBox86|HitBox87";
	rename -uid "4A4D2B64-234C-3326-EBC9-21B9FEB34EA8";
	setAttr ".rp" -type "double3" -16 2 16 ;
	setAttr ".sp" -type "double3" -16 2 16 ;
createNode mesh -n "HitBox88Shape" -p "|Queen|HitBox85|HitBox86|HitBox87|HitBox88";
	rename -uid "53BD84A2-0E4B-F8D2-FBBA-DDB122093059";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -16 2 16 -16 2 16 -16 2 16 
		-16 2 16 -16 2 16 -16 2 16 -16 2 16 -16 2 16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox89" -p "|Queen|HitBox85|HitBox86|HitBox87|HitBox88";
	rename -uid "0BBCECAF-9D48-7332-E3E6-6F84A77DA9F4";
	setAttr ".rp" -type "double3" -20 2 20 ;
	setAttr ".sp" -type "double3" -20 2 20 ;
createNode mesh -n "HitBox89Shape" -p "|Queen|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89";
	rename -uid "8F8CE999-654E-07A4-110B-79A1BAD55947";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -20 2 20 -20 2 20 -20 2 20 
		-20 2 20 -20 2 20 -20 2 20 -20 2 20 -20 2 20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox90" -p "|Queen|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89";
	rename -uid "144E4537-BE46-9CBA-32DC-BCAD62CD6AF8";
	setAttr ".rp" -type "double3" -24 2 24 ;
	setAttr ".sp" -type "double3" -24 2 24 ;
createNode mesh -n "HitBox90Shape" -p "|Queen|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox90";
	rename -uid "43A1CB1A-EE49-F43E-BFA2-2E85AAE06BAE";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -24 2 24 -24 2 24 -24 2 24 
		-24 2 24 -24 2 24 -24 2 24 -24 2 24 -24 2 24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox91" -p "|Queen|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox90";
	rename -uid "1095FD66-434C-8F8E-088D-1ABBA67B5C26";
	setAttr ".rp" -type "double3" -28 2 28 ;
	setAttr ".sp" -type "double3" -28 2 28 ;
createNode mesh -n "HitBox91Shape" -p "|Queen|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox90|HitBox91";
	rename -uid "9183A914-FC41-7931-F8DB-BCAD5ED85D43";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -28 2 28 -28 2 28 -28 2 28 
		-28 2 28 -28 2 28 -28 2 28 -28 2 28 -28 2 28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox92" -p "Queen";
	rename -uid "750C4147-CA49-B334-B476-B295D3E3AD30";
	setAttr ".rp" -type "double3" -4 2 -4 ;
	setAttr ".sp" -type "double3" -4 2 -4 ;
createNode mesh -n "HitBox92Shape" -p "|Queen|HitBox92";
	rename -uid "3C7C9438-804D-8E20-7F0F-B687A4C56F51";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 -4 -4 2 -4 -4 2 -4 -4 
		2 -4 -4 2 -4 -4 2 -4 -4 2 -4 -4 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox93" -p "|Queen|HitBox92";
	rename -uid "381C68D1-0B4D-6E82-2911-279F9CDF3EA1";
	setAttr ".rp" -type "double3" -8 2 -8 ;
	setAttr ".sp" -type "double3" -8 2 -8 ;
createNode mesh -n "HitBox93Shape" -p "|Queen|HitBox92|HitBox93";
	rename -uid "4DABF202-EA4A-C5BF-DDF3-00835EB3A80B";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -8 2 -8 -8 2 -8 -8 2 -8 -8 
		2 -8 -8 2 -8 -8 2 -8 -8 2 -8 -8 2 -8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox94" -p "|Queen|HitBox92|HitBox93";
	rename -uid "E3EB4CB8-3F4C-3219-F03E-EB8E1F09ADE1";
	setAttr ".rp" -type "double3" -12 2 -12 ;
	setAttr ".sp" -type "double3" -12 2 -12 ;
createNode mesh -n "HitBox94Shape" -p "|Queen|HitBox92|HitBox93|HitBox94";
	rename -uid "7AC63781-7D4C-323F-4848-CE97FCF26143";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -12 2 -12 -12 2 -12 -12 2 
		-12 -12 2 -12 -12 2 -12 -12 2 -12 -12 2 -12 -12 2 -12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox95" -p "|Queen|HitBox92|HitBox93|HitBox94";
	rename -uid "17E96692-5F47-0F5E-E993-A89B3CE60BF1";
	setAttr ".rp" -type "double3" -16 2 -16 ;
	setAttr ".sp" -type "double3" -16 2 -16 ;
createNode mesh -n "HitBox95Shape" -p "|Queen|HitBox92|HitBox93|HitBox94|HitBox95";
	rename -uid "DB705D96-904E-3B5C-D61D-9F80DBC4425C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -16 2 -16 -16 2 -16 -16 2 
		-16 -16 2 -16 -16 2 -16 -16 2 -16 -16 2 -16 -16 2 -16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox96" -p "|Queen|HitBox92|HitBox93|HitBox94|HitBox95";
	rename -uid "3FEAEFA2-D342-6DAF-71EE-63A042ADB334";
	setAttr ".rp" -type "double3" -20 2 -20 ;
	setAttr ".sp" -type "double3" -20 2 -20 ;
createNode mesh -n "HitBox96Shape" -p "|Queen|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96";
	rename -uid "3D2917F7-3B44-FDB9-750F-1FACE797F93A";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -20 2 -20 -20 2 -20 -20 2 
		-20 -20 2 -20 -20 2 -20 -20 2 -20 -20 2 -20 -20 2 -20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox97" -p "|Queen|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96";
	rename -uid "C534C474-6D4D-94CE-5B65-B48402846363";
	setAttr ".rp" -type "double3" -24 2 -24 ;
	setAttr ".sp" -type "double3" -24 2 -24 ;
createNode mesh -n "HitBox97Shape" -p "|Queen|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox97";
	rename -uid "39D243EE-3C41-83C1-97D1-3C8BE8565EFF";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -24 2 -24 -24 2 -24 -24 2 
		-24 -24 2 -24 -24 2 -24 -24 2 -24 -24 2 -24 -24 2 -24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox98" -p "|Queen|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox97";
	rename -uid "1C8CE3BD-C943-5768-D2CC-89AD80AFA7FC";
	setAttr ".rp" -type "double3" -28 2 -28 ;
	setAttr ".sp" -type "double3" -28 2 -28 ;
createNode mesh -n "HitBox98Shape" -p "|Queen|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox97|HitBox98";
	rename -uid "E4153457-9349-6A64-335F-0E89C2F6A806";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -28 2 -28 -28 2 -28 -28 2 
		-28 -28 2 -28 -28 2 -28 -28 2 -28 -28 2 -28 -28 2 -28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox99" -p "Queen";
	rename -uid "CFA4CF0F-054F-DCFF-0305-F9ABE9DC378D";
	setAttr ".rp" -type "double3" 0 2 -4 ;
	setAttr ".sp" -type "double3" 0 2 -4 ;
createNode mesh -n "HitBox99Shape" -p "|Queen|HitBox99";
	rename -uid "D58FC728-B04B-8B69-C58D-D3A6214CA7B2";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -4 0 2 -4 0 2 -4 0 2 
		-4 0 2 -4 0 2 -4 0 2 -4 0 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox57" -p "|Queen|HitBox99";
	rename -uid "E991650D-3D45-2E48-F3F2-10948D211F0A";
	setAttr ".rp" -type "double3" 0 2 -8 ;
	setAttr ".sp" -type "double3" 0 2 -8 ;
createNode mesh -n "HitBox57Shape" -p "|Queen|HitBox99|HitBox57";
	rename -uid "1D82B5A6-DF4E-0554-0E76-99908A082E68";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -8 0 2 -8 0 2 -8 0 2 
		-8 0 2 -8 0 2 -8 0 2 -8 0 2 -8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox56" -p "|Queen|HitBox99|HitBox57";
	rename -uid "5532EEAF-FA46-6BC7-4112-65B2A1B0C4D8";
	setAttr ".rp" -type "double3" 0 2 -12 ;
	setAttr ".sp" -type "double3" 0 2 -12 ;
createNode mesh -n "HitBox56Shape" -p "|Queen|HitBox99|HitBox57|HitBox56";
	rename -uid "B079D923-D141-67DB-62B5-87AE34C7E951";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -12 0 2 -12 0 2 -12 0 
		2 -12 0 2 -12 0 2 -12 0 2 -12 0 2 -12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox102" -p "|Queen|HitBox99|HitBox57|HitBox56";
	rename -uid "3ED59053-814D-AFE2-DDE4-7389E9EA6B04";
	setAttr ".rp" -type "double3" 0 2 -16 ;
	setAttr ".sp" -type "double3" 0 2 -16 ;
createNode mesh -n "HitBox102Shape" -p "|Queen|HitBox99|HitBox57|HitBox56|HitBox102";
	rename -uid "14A076F4-944F-F9BF-D82C-D3BB5B807D1D";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -16 0 2 -16 0 2 -16 0 
		2 -16 0 2 -16 0 2 -16 0 2 -16 0 2 -16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox99|HitBox57|HitBox56|HitBox102";
	rename -uid "12361BC7-F141-F5D6-DB53-2B9F0BFC4A54";
	setAttr ".rp" -type "double3" 0 2 -20 ;
	setAttr ".sp" -type "double3" 0 2 -20 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox99|HitBox57|HitBox56|HitBox102|HitBox";
	rename -uid "94B77B61-6A45-0BDC-AE7F-9B804131A07C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -20 0 2 -20 0 2 -20 0 
		2 -20 0 2 -20 0 2 -20 0 2 -20 0 2 -20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox99|HitBox57|HitBox56|HitBox102|HitBox";
	rename -uid "EF8630FE-834F-0475-32B4-15BED9F2D01E";
	setAttr ".rp" -type "double3" 0 2 -24 ;
	setAttr ".sp" -type "double3" 0 2 -24 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBox";
	rename -uid "AF365CD8-1E4E-0184-9267-0C9CB778AD94";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -24 0 2 -24 0 2 -24 0 
		2 -24 0 2 -24 0 2 -24 0 2 -24 0 2 -24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBox";
	rename -uid "3D3D913A-244D-A14D-67C1-CCA39F060113";
	setAttr ".rp" -type "double3" 0 2 -28 ;
	setAttr ".sp" -type "double3" 0 2 -28 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBox|HitBox";
	rename -uid "F583D675-4C44-752B-F13E-6BBF46713E84";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -28 0 2 -28 0 2 -28 0 
		2 -28 0 2 -28 0 2 -28 0 2 -28 0 2 -28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox0" -p "Queen";
	rename -uid "1740E7A5-8B4A-D463-0F7D-AF8585518DFA";
	setAttr ".rp" -type "double3" 4 2 -4 ;
	setAttr ".sp" -type "double3" 4 2 -4 ;
createNode mesh -n "HitBox0Shape" -p "|Queen|HitBox0";
	rename -uid "78F4EE55-A247-2E8C-CC96-59A242DC1675";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 -4 4 2 -4 4 2 -4 4 2 
		-4 4 2 -4 4 2 -4 4 2 -4 4 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox0";
	rename -uid "AA8DF113-9D43-3FB0-1B33-26A85874AB10";
	setAttr ".rp" -type "double3" 8 2 -8 ;
	setAttr ".sp" -type "double3" 8 2 -8 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox0|HitBox";
	rename -uid "B8A8480A-9542-1141-F40B-099B226A1B39";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  8 2 -8 8 2 -8 8 2 -8 8 2 
		-8 8 2 -8 8 2 -8 8 2 -8 8 2 -8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox0|HitBox";
	rename -uid "66DC77C0-6A47-7EE7-903D-999F5608E65B";
	setAttr ".rp" -type "double3" 12 2 -12 ;
	setAttr ".sp" -type "double3" 12 2 -12 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox0|HitBox|HitBox";
	rename -uid "B8C8E8CF-AF47-C4A2-7522-29A3B0872A14";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  12 2 -12 12 2 -12 12 2 -12 
		12 2 -12 12 2 -12 12 2 -12 12 2 -12 12 2 -12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox0|HitBox|HitBox";
	rename -uid "46EFB252-CD47-43C4-D3DD-3BBB52462185";
	setAttr ".rp" -type "double3" 16 2 -16 ;
	setAttr ".sp" -type "double3" 16 2 -16 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox0|HitBox|HitBox|HitBox";
	rename -uid "E6B05A28-F545-474F-CD1A-53A836D3D686";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  16 2 -16 16 2 -16 16 2 -16 
		16 2 -16 16 2 -16 16 2 -16 16 2 -16 16 2 -16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox0|HitBox|HitBox|HitBox";
	rename -uid "7CF903E4-BE40-ED79-1699-7A9179C766BE";
	setAttr ".rp" -type "double3" 20 2 -20 ;
	setAttr ".sp" -type "double3" 20 2 -20 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox0|HitBox|HitBox|HitBox|HitBox";
	rename -uid "A951F2A7-9444-2EE4-F56B-2DA46328EADF";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  20 2 -20 20 2 -20 20 2 -20 
		20 2 -20 20 2 -20 20 2 -20 20 2 -20 20 2 -20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox0|HitBox|HitBox|HitBox|HitBox";
	rename -uid "BF0AAD2C-DC41-7E27-B12B-BC8093276796";
	setAttr ".rp" -type "double3" 24 2 -24 ;
	setAttr ".sp" -type "double3" 24 2 -24 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "7278BFB3-294E-CC7E-568E-F5A22BCA7CDD";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  24 2 -24 24 2 -24 24 2 -24 
		24 2 -24 24 2 -24 24 2 -24 24 2 -24 24 2 -24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Queen|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "C95D4D91-B146-2711-D72B-528603E49E88";
	setAttr ".rp" -type "double3" 28 2 -28 ;
	setAttr ".sp" -type "double3" 28 2 -28 ;
createNode mesh -n "HitBoxShape" -p "|Queen|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "50A4A969-9B48-619E-052D-32ADEFC8577D";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  28 2 -28 28 2 -28 28 2 -28 
		28 2 -28 28 2 -28 28 2 -28 28 2 -28 28 2 -28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "Knight";
	rename -uid "3858CCBF-5C48-3148-057F-469FA5C08A53";
	setAttr ".t" -type "double3" 0 0 -115 ;
createNode transform -n "HitBox35" -p "Knight";
	rename -uid "86B4850D-2149-8016-6DE7-DFB9FB2451FD";
	setAttr ".rp" -type "double3" -4 2 8 ;
	setAttr ".sp" -type "double3" -4 2 8 ;
createNode mesh -n "HitBox35Shape" -p "|Knight|HitBox35";
	rename -uid "345CAD85-6F45-7683-75FF-C8920100EF0C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 8 -4 2 8 -4 2 8 -4 2 
		8 -4 2 8 -4 2 8 -4 2 8 -4 2 8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox60" -p "Knight";
	rename -uid "335CDC8C-4146-1B89-C1FE-7EAA90BCCAE5";
	setAttr ".rp" -type "double3" 8 2 4 ;
	setAttr ".sp" -type "double3" 8 2 4 ;
createNode mesh -n "HitBox60Shape" -p "|Knight|HitBox60";
	rename -uid "A0B59A3E-C445-AC63-D03F-41960D9CB25F";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  8 2 4 8 2 4 8 2 4 8 2 4 8 
		2 4 8 2 4 8 2 4 8 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox56" -p "Knight";
	rename -uid "575AA83D-EB47-94FA-D743-AFB2F477A7C7";
	setAttr ".rp" -type "double3" 8 2 -4 ;
	setAttr ".sp" -type "double3" 8 2 -4 ;
createNode mesh -n "HitBox56Shape" -p "|Knight|HitBox56";
	rename -uid "3C2A24F7-694B-3B8A-D437-1994D554EF07";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  8 2 -4 8 2 -4 8 2 -4 8 2 
		-4 8 2 -4 8 2 -4 8 2 -4 8 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox54" -p "Knight";
	rename -uid "BF4DE952-E74D-4B65-81F3-B28C3838C55C";
	setAttr ".rp" -type "double3" 4 2 -8 ;
	setAttr ".sp" -type "double3" 4 2 -8 ;
createNode mesh -n "HitBox54Shape" -p "|Knight|HitBox54";
	rename -uid "20183992-0443-3C06-138B-3CBF44B25AD4";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 -8 4 2 -8 4 2 -8 4 2 
		-8 4 2 -8 4 2 -8 4 2 -8 4 2 -8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox51" -p "Knight";
	rename -uid "325C6FE5-2645-F127-5C3A-61936C072D64";
	setAttr ".rp" -type "double3" 4 2 8 ;
	setAttr ".sp" -type "double3" 4 2 8 ;
createNode mesh -n "HitBox51Shape" -p "|Knight|HitBox51";
	rename -uid "9FE5674A-5D49-916D-ED7C-E9AF0F1967F3";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 8 4 2 8 4 2 8 4 2 8 4 
		2 8 4 2 8 4 2 8 4 2 8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox28" -p "Knight";
	rename -uid "157DA61C-BD4F-09D0-2C71-D88630DDD806";
	setAttr ".rp" -type "double3" -8 2 4 ;
	setAttr ".sp" -type "double3" -8 2 4 ;
createNode mesh -n "HitBox28Shape" -p "|Knight|HitBox28";
	rename -uid "2528CCD0-B847-A00F-B975-74BE9040CE8A";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -8 2 4 -8 2 4 -8 2 4 -8 2 
		4 -8 2 4 -8 2 4 -8 2 4 -8 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox24" -p "Knight";
	rename -uid "B4899F98-6D49-D9DF-6E5D-C1B6A1C1EBFC";
	setAttr ".rp" -type "double3" -8 2 -4 ;
	setAttr ".sp" -type "double3" -8 2 -4 ;
createNode mesh -n "HitBox24Shape" -p "|Knight|HitBox24";
	rename -uid "EA9BB37F-294B-52D7-09A3-1FA527374207";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -8 2 -4 -8 2 -4 -8 2 -4 -8 
		2 -4 -8 2 -4 -8 2 -4 -8 2 -4 -8 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "HitBox38" -p "Knight";
	rename -uid "E0519279-4543-750C-AE7F-0594345B5562";
	setAttr ".rp" -type "double3" -4 2 -8 ;
	setAttr ".sp" -type "double3" -4 2 -8 ;
createNode mesh -n "HitBox38Shape" -p "|Knight|HitBox38";
	rename -uid "062A1ACD-1942-A219-A01B-51B5BC31D1D8";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 -8 -4 2 -8 -4 2 -8 -4 
		2 -8 -4 2 -8 -4 2 -8 -4 2 -8 -4 2 -8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "Bishop";
	rename -uid "517D6B09-9847-BAED-6604-D8AE1D1B6283";
	setAttr ".t" -type "double3" 0 0 67 ;
createNode transform -n "HitBox78" -p "Bishop";
	rename -uid "86884CCE-1C48-1A0B-1184-71AD64DA17E7";
	setAttr ".rp" -type "double3" 4 2 4 ;
	setAttr ".sp" -type "double3" 4 2 4 ;
createNode mesh -n "HitBox78Shape" -p "|Bishop|HitBox78";
	rename -uid "E294E257-C742-22FD-F820-398E0EC5A593";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 4 4 2 4 4 2 4 4 2 4 4 
		2 4 4 2 4 4 2 4 4 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox79" -p "|Bishop|HitBox78";
	rename -uid "A2511719-4046-7BDA-CFB7-3AAC17681527";
	setAttr ".rp" -type "double3" 8 2 8 ;
	setAttr ".sp" -type "double3" 8 2 8 ;
createNode mesh -n "HitBox79Shape" -p "|Bishop|HitBox78|HitBox79";
	rename -uid "52AECFA0-DE40-9389-5782-B2B14BB8E1D6";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  8 2 8 8 2 8 8 2 8 8 2 8 8 
		2 8 8 2 8 8 2 8 8 2 8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox80" -p "|Bishop|HitBox78|HitBox79";
	rename -uid "983107C0-2C43-F196-C9A6-A593AA3C1826";
	setAttr ".rp" -type "double3" 12 2 12 ;
	setAttr ".sp" -type "double3" 12 2 12 ;
createNode mesh -n "HitBox80Shape" -p "|Bishop|HitBox78|HitBox79|HitBox80";
	rename -uid "9EFC8E82-DC4E-A0F2-44A5-BBAE0300F552";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  12 2 12 12 2 12 12 2 12 12 
		2 12 12 2 12 12 2 12 12 2 12 12 2 12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox81" -p "|Bishop|HitBox78|HitBox79|HitBox80";
	rename -uid "51CFC67C-9D4E-0CCF-149A-17BEC851342A";
	setAttr ".rp" -type "double3" 16 2 16 ;
	setAttr ".sp" -type "double3" 16 2 16 ;
createNode mesh -n "HitBox81Shape" -p "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81";
	rename -uid "44965C56-EF43-EFC3-E5F2-8796A6B29B62";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  16 2 16 16 2 16 16 2 16 16 
		2 16 16 2 16 16 2 16 16 2 16 16 2 16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox82" -p "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81";
	rename -uid "D5F43D3B-1E49-02CC-864D-59A8EA927BB2";
	setAttr ".rp" -type "double3" 20 2 20 ;
	setAttr ".sp" -type "double3" 20 2 20 ;
createNode mesh -n "HitBox82Shape" -p "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82";
	rename -uid "E365D58F-8F44-CE2B-FB56-78B62B3D2653";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  20 2 20 20 2 20 20 2 20 20 
		2 20 20 2 20 20 2 20 20 2 20 20 2 20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox83" -p "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82";
	rename -uid "E587922F-9F4F-E94B-D321-C5889D3EE531";
	setAttr ".rp" -type "double3" 24 2 24 ;
	setAttr ".sp" -type "double3" 24 2 24 ;
createNode mesh -n "HitBox83Shape" -p "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox83";
	rename -uid "94C7D219-B44D-90C6-BF6F-1BB0D8B8BB79";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  24 2 24 24 2 24 24 2 24 24 
		2 24 24 2 24 24 2 24 24 2 24 24 2 24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox84" -p "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox83";
	rename -uid "1C2FF6C6-9941-9D72-3AF4-77862606BB40";
	setAttr ".rp" -type "double3" 28 2 28 ;
	setAttr ".sp" -type "double3" 28 2 28 ;
createNode mesh -n "HitBox84Shape" -p "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox83|HitBox84";
	rename -uid "6D3194C2-2947-870C-8C25-55A43F7A9450";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  28 2 28 28 2 28 28 2 28 28 
		2 28 28 2 28 28 2 28 28 2 28 28 2 28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox85" -p "Bishop";
	rename -uid "9A733100-2F41-7787-B421-6DAD1538B70F";
	setAttr ".rp" -type "double3" -4 2 4 ;
	setAttr ".sp" -type "double3" -4 2 4 ;
createNode mesh -n "HitBox85Shape" -p "|Bishop|HitBox85";
	rename -uid "8E54A078-774F-3649-1519-6EB403C62562";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 4 -4 2 4 -4 2 4 -4 2 
		4 -4 2 4 -4 2 4 -4 2 4 -4 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox86" -p "|Bishop|HitBox85";
	rename -uid "6ED871D4-BF47-8F77-F89F-00907E6332FC";
	setAttr ".rp" -type "double3" -8 2 8 ;
	setAttr ".sp" -type "double3" -8 2 8 ;
createNode mesh -n "HitBox86Shape" -p "|Bishop|HitBox85|HitBox86";
	rename -uid "C7E6CFB4-EC4D-3217-348A-248B8729AA80";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -8 2 8 -8 2 8 -8 2 8 -8 2 
		8 -8 2 8 -8 2 8 -8 2 8 -8 2 8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox87" -p "|Bishop|HitBox85|HitBox86";
	rename -uid "E4A50FC8-7147-49AC-BC80-9B8BE8B3E431";
	setAttr ".rp" -type "double3" -12 2 12 ;
	setAttr ".sp" -type "double3" -12 2 12 ;
createNode mesh -n "HitBox87Shape" -p "|Bishop|HitBox85|HitBox86|HitBox87";
	rename -uid "0A10E00C-5B46-E24C-69C3-F18C1EDF2933";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -12 2 12 -12 2 12 -12 2 12 
		-12 2 12 -12 2 12 -12 2 12 -12 2 12 -12 2 12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox88" -p "|Bishop|HitBox85|HitBox86|HitBox87";
	rename -uid "342A40B0-194E-76FB-732F-CB91270CD6DD";
	setAttr ".rp" -type "double3" -16 2 16 ;
	setAttr ".sp" -type "double3" -16 2 16 ;
createNode mesh -n "HitBox88Shape" -p "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88";
	rename -uid "EB6C2044-A44B-0BBE-70F3-16B2C188EEA7";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -16 2 16 -16 2 16 -16 2 16 
		-16 2 16 -16 2 16 -16 2 16 -16 2 16 -16 2 16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox89" -p "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88";
	rename -uid "4C6DD347-264F-0470-90CC-5F92BA480911";
	setAttr ".rp" -type "double3" -20 2 20 ;
	setAttr ".sp" -type "double3" -20 2 20 ;
createNode mesh -n "HitBox89Shape" -p "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89";
	rename -uid "7F0BFB32-F849-A908-DA6A-9C8579F6E03A";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -20 2 20 -20 2 20 -20 2 20 
		-20 2 20 -20 2 20 -20 2 20 -20 2 20 -20 2 20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox90" -p "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89";
	rename -uid "ACB5E2C3-6741-8A62-6190-188893D834D6";
	setAttr ".rp" -type "double3" -24 2 24 ;
	setAttr ".sp" -type "double3" -24 2 24 ;
createNode mesh -n "HitBox90Shape" -p "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox90";
	rename -uid "6FEE4F5F-F541-FD0A-968E-1BAAC4E91DDD";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -24 2 24 -24 2 24 -24 2 24 
		-24 2 24 -24 2 24 -24 2 24 -24 2 24 -24 2 24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox91" -p "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox90";
	rename -uid "2DFCBD25-3C4B-E333-9835-EEB90685838E";
	setAttr ".rp" -type "double3" -28 2 28 ;
	setAttr ".sp" -type "double3" -28 2 28 ;
createNode mesh -n "HitBox91Shape" -p "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox90|HitBox91";
	rename -uid "AF537827-5347-5FAE-E0AB-F8BCEEC33CED";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -28 2 28 -28 2 28 -28 2 28 
		-28 2 28 -28 2 28 -28 2 28 -28 2 28 -28 2 28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox92" -p "Bishop";
	rename -uid "0852D7E0-8F43-447D-74EB-3CA77ECFF66C";
	setAttr ".rp" -type "double3" -4 2 -4 ;
	setAttr ".sp" -type "double3" -4 2 -4 ;
createNode mesh -n "HitBox92Shape" -p "|Bishop|HitBox92";
	rename -uid "4157DF79-4848-986B-82A0-7F97479CC781";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 -4 -4 2 -4 -4 2 -4 -4 
		2 -4 -4 2 -4 -4 2 -4 -4 2 -4 -4 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox93" -p "|Bishop|HitBox92";
	rename -uid "9B75B4FA-B444-989C-0C15-2FA8E6E725EE";
	setAttr ".rp" -type "double3" -8 2 -8 ;
	setAttr ".sp" -type "double3" -8 2 -8 ;
createNode mesh -n "HitBox93Shape" -p "|Bishop|HitBox92|HitBox93";
	rename -uid "9B1A4762-964B-B773-4745-3D90F2B8988D";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -8 2 -8 -8 2 -8 -8 2 -8 -8 
		2 -8 -8 2 -8 -8 2 -8 -8 2 -8 -8 2 -8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox94" -p "|Bishop|HitBox92|HitBox93";
	rename -uid "38E6779C-334D-A473-7683-0191CE773F49";
	setAttr ".rp" -type "double3" -12 2 -12 ;
	setAttr ".sp" -type "double3" -12 2 -12 ;
createNode mesh -n "HitBox94Shape" -p "|Bishop|HitBox92|HitBox93|HitBox94";
	rename -uid "AF4763BE-5D4E-CCC9-98F9-5C92A9DA73B3";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -12 2 -12 -12 2 -12 -12 2 
		-12 -12 2 -12 -12 2 -12 -12 2 -12 -12 2 -12 -12 2 -12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox95" -p "|Bishop|HitBox92|HitBox93|HitBox94";
	rename -uid "93B22D10-E147-9036-3728-029DA4B99F8D";
	setAttr ".rp" -type "double3" -16 2 -16 ;
	setAttr ".sp" -type "double3" -16 2 -16 ;
createNode mesh -n "HitBox95Shape" -p "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95";
	rename -uid "29B3ED9B-F847-BA1E-9691-0A8A7E894AA7";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -16 2 -16 -16 2 -16 -16 2 
		-16 -16 2 -16 -16 2 -16 -16 2 -16 -16 2 -16 -16 2 -16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox96" -p "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95";
	rename -uid "5D266E86-8744-4D5F-C848-C084DB3D6DFD";
	setAttr ".rp" -type "double3" -20 2 -20 ;
	setAttr ".sp" -type "double3" -20 2 -20 ;
createNode mesh -n "HitBox96Shape" -p "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96";
	rename -uid "CBB3EEDD-E941-76D7-EE45-F2815FB9F2F3";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -20 2 -20 -20 2 -20 -20 2 
		-20 -20 2 -20 -20 2 -20 -20 2 -20 -20 2 -20 -20 2 -20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox97" -p "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96";
	rename -uid "583A070E-BC4D-E778-0782-67A8F8E07877";
	setAttr ".rp" -type "double3" -24 2 -24 ;
	setAttr ".sp" -type "double3" -24 2 -24 ;
createNode mesh -n "HitBox97Shape" -p "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox97";
	rename -uid "271A03CD-4848-97F0-1990-58A30516C4C5";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -24 2 -24 -24 2 -24 -24 2 
		-24 -24 2 -24 -24 2 -24 -24 2 -24 -24 2 -24 -24 2 -24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox98" -p "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox97";
	rename -uid "0E741E8B-4B40-C2AF-4DB0-E9833A49D2FE";
	setAttr ".rp" -type "double3" -28 2 -28 ;
	setAttr ".sp" -type "double3" -28 2 -28 ;
createNode mesh -n "HitBox98Shape" -p "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox97|HitBox98";
	rename -uid "068CF431-C542-839F-C6FD-EDB9243E1023";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -28 2 -28 -28 2 -28 -28 2 
		-28 -28 2 -28 -28 2 -28 -28 2 -28 -28 2 -28 -28 2 -28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox0" -p "Bishop";
	rename -uid "25E1C5B8-2A43-F429-53D1-87B441B37453";
	setAttr ".rp" -type "double3" 4 2 -4 ;
	setAttr ".sp" -type "double3" 4 2 -4 ;
createNode mesh -n "HitBox0Shape" -p "|Bishop|HitBox0";
	rename -uid "D41408E0-D040-E3C2-1F1A-E5951476783E";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 -4 4 2 -4 4 2 -4 4 2 
		-4 4 2 -4 4 2 -4 4 2 -4 4 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Bishop|HitBox0";
	rename -uid "395F4AE6-C145-71EC-53B6-E79DFFE7B79A";
	setAttr ".rp" -type "double3" 8 2 -8 ;
	setAttr ".sp" -type "double3" 8 2 -8 ;
createNode mesh -n "HitBoxShape" -p "|Bishop|HitBox0|HitBox";
	rename -uid "64119C19-E542-968B-A7E1-7CA900C9538C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  8 2 -8 8 2 -8 8 2 -8 8 2 
		-8 8 2 -8 8 2 -8 8 2 -8 8 2 -8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Bishop|HitBox0|HitBox";
	rename -uid "DDC56F84-7242-3C42-54A6-0EAB96EEF7CA";
	setAttr ".rp" -type "double3" 12 2 -12 ;
	setAttr ".sp" -type "double3" 12 2 -12 ;
createNode mesh -n "HitBoxShape" -p "|Bishop|HitBox0|HitBox|HitBox";
	rename -uid "E9E6C832-594A-4633-C69E-3188728CD8DC";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  12 2 -12 12 2 -12 12 2 -12 
		12 2 -12 12 2 -12 12 2 -12 12 2 -12 12 2 -12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Bishop|HitBox0|HitBox|HitBox";
	rename -uid "8E9C494F-3045-4690-1E31-3E851E354C69";
	setAttr ".rp" -type "double3" 16 2 -16 ;
	setAttr ".sp" -type "double3" 16 2 -16 ;
createNode mesh -n "HitBoxShape" -p "|Bishop|HitBox0|HitBox|HitBox|HitBox";
	rename -uid "B45EC564-D542-A365-3939-928B8839D73C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  16 2 -16 16 2 -16 16 2 -16 
		16 2 -16 16 2 -16 16 2 -16 16 2 -16 16 2 -16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Bishop|HitBox0|HitBox|HitBox|HitBox";
	rename -uid "0BAD40B4-6E44-C9A5-4E4D-D693875AB598";
	setAttr ".rp" -type "double3" 20 2 -20 ;
	setAttr ".sp" -type "double3" 20 2 -20 ;
createNode mesh -n "HitBoxShape" -p "|Bishop|HitBox0|HitBox|HitBox|HitBox|HitBox";
	rename -uid "D6C856BE-9C41-0550-5CEE-6EAEFEC900CD";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  20 2 -20 20 2 -20 20 2 -20 
		20 2 -20 20 2 -20 20 2 -20 20 2 -20 20 2 -20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Bishop|HitBox0|HitBox|HitBox|HitBox|HitBox";
	rename -uid "FFF724D8-3743-2F8F-6D92-8CA5C47C373C";
	setAttr ".rp" -type "double3" 24 2 -24 ;
	setAttr ".sp" -type "double3" 24 2 -24 ;
createNode mesh -n "HitBoxShape" -p "|Bishop|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "3BB16062-9B43-7C6B-A18D-5BBF92FF82D5";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  24 2 -24 24 2 -24 24 2 -24 
		24 2 -24 24 2 -24 24 2 -24 24 2 -24 24 2 -24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Bishop|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "8A684697-2E4C-32AE-19BA-2E93C371A5B1";
	setAttr ".rp" -type "double3" 28 2 -28 ;
	setAttr ".sp" -type "double3" 28 2 -28 ;
createNode mesh -n "HitBoxShape" -p "|Bishop|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "1C370B2F-E64B-AE60-2015-1688C8F70E91";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  28 2 -28 28 2 -28 28 2 -28 
		28 2 -28 28 2 -28 28 2 -28 28 2 -28 28 2 -28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "Rook";
	rename -uid "6F3FE932-DB4B-38F1-A96E-CFB6639319DD";
	setAttr ".t" -type "double3" 65 0 -75.999999999999986 ;
createNode transform -n "HitBox51" -p "Rook";
	rename -uid "DB6811C7-7D4D-2FEC-E269-B4831DD3E0DD";
	setAttr ".rp" -type "double3" -4 2 0 ;
	setAttr ".sp" -type "double3" -4 2 0 ;
createNode mesh -n "HitBox51Shape" -p "|Rook|HitBox51";
	rename -uid "B53F8819-2E4B-562F-1D7B-66907DD5C851";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 0 -4 2 0 -4 2 0 -4 2 
		0 -4 2 0 -4 2 0 -4 2 0 -4 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox43" -p "|Rook|HitBox51";
	rename -uid "0A8096FE-1E43-82F6-0F17-2780134CBBF2";
	setAttr ".rp" -type "double3" -8 2 0 ;
	setAttr ".sp" -type "double3" -8 2 0 ;
createNode mesh -n "HitBox43Shape" -p "|Rook|HitBox51|HitBox43";
	rename -uid "FD58B707-964C-DA62-6FF1-388DAFA65D42";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -8 2 0 -8 2 0 -8 2 0 -8 2 
		0 -8 2 0 -8 2 0 -8 2 0 -8 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox35" -p "|Rook|HitBox51|HitBox43";
	rename -uid "3B55F91D-C74D-1EAD-E126-60A888A64C59";
	setAttr ".rp" -type "double3" -12 2 0 ;
	setAttr ".sp" -type "double3" -12 2 0 ;
createNode mesh -n "HitBox35Shape" -p "|Rook|HitBox51|HitBox43|HitBox35";
	rename -uid "8D58CE68-CA40-D424-9D35-86A2BC859769";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -12 2 0 -12 2 0 -12 2 0 -12 
		2 0 -12 2 0 -12 2 0 -12 2 0 -12 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox27" -p "|Rook|HitBox51|HitBox43|HitBox35";
	rename -uid "E21066AE-B049-ED86-7655-E28B38B523FF";
	setAttr ".rp" -type "double3" -16 2 0 ;
	setAttr ".sp" -type "double3" -16 2 0 ;
createNode mesh -n "HitBox27Shape" -p "|Rook|HitBox51|HitBox43|HitBox35|HitBox27";
	rename -uid "D4F5EBF9-2745-95B0-6065-92812D36A582";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -16 2 0 -16 2 0 -16 2 0 -16 
		2 0 -16 2 0 -16 2 0 -16 2 0 -16 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox19" -p "|Rook|HitBox51|HitBox43|HitBox35|HitBox27";
	rename -uid "FC631317-F94F-020F-5CD5-248D2FA42842";
	setAttr ".rp" -type "double3" -20 2 0 ;
	setAttr ".sp" -type "double3" -20 2 0 ;
createNode mesh -n "HitBox19Shape" -p "|Rook|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19";
	rename -uid "922F6B0C-5E40-80AA-9051-8695FB581E0E";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -20 2 0 -20 2 0 -20 2 0 -20 
		2 0 -20 2 0 -20 2 0 -20 2 0 -20 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox11" -p "|Rook|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19";
	rename -uid "788CAC73-344E-7551-1655-A7931F0E78C2";
	setAttr ".rp" -type "double3" -24 2 0 ;
	setAttr ".sp" -type "double3" -24 2 0 ;
createNode mesh -n "HitBox11Shape" -p "|Rook|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox11";
	rename -uid "CF5C5CD7-5346-C862-6666-308C8C7E8A1F";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -24 2 0 -24 2 0 -24 2 0 -24 
		2 0 -24 2 0 -24 2 0 -24 2 0 -24 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox7" -p "|Rook|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox11";
	rename -uid "45F44436-2C40-3E58-77D5-D9A8D3E304F7";
	setAttr ".rp" -type "double3" -28 2 0 ;
	setAttr ".sp" -type "double3" -28 2 0 ;
createNode mesh -n "HitBox7Shape" -p "|Rook|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox11|HitBox7";
	rename -uid "03D3D51F-724D-E0DB-C503-DEB512C0BA51";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -28 2 0 -28 2 0 -28 2 0 -28 
		2 0 -28 2 0 -28 2 0 -28 2 0 -28 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox113" -p "Rook";
	rename -uid "B4442D69-0E4B-DD6B-86A7-F4BDA2CDB04D";
	setAttr ".rp" -type "double3" 4 2 0 ;
	setAttr ".sp" -type "double3" 4 2 0 ;
createNode mesh -n "HitBox113Shape" -p "|Rook|HitBox113";
	rename -uid "80294DD0-B747-EBEE-CDF2-9F92CC306D30";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 0 4 2 0 4 2 0 4 2 0 4 
		2 0 4 2 0 4 2 0 4 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Rook|HitBox113";
	rename -uid "A859E523-3B44-7644-005B-09B10BC9D4F4";
	setAttr ".rp" -type "double3" 8 2 0 ;
	setAttr ".sp" -type "double3" 8 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Rook|HitBox113|HitBox";
	rename -uid "C931B5CD-1E4B-9A7E-6DE1-D684523E4600";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  8 2 0 8 2 0 8 2 0 8 2 0 8 
		2 0 8 2 0 8 2 0 8 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Rook|HitBox113|HitBox";
	rename -uid "917A12DF-414E-11E8-101D-A9B775DA0EAC";
	setAttr ".rp" -type "double3" 12 2 0 ;
	setAttr ".sp" -type "double3" 12 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Rook|HitBox113|HitBox|HitBox";
	rename -uid "CEECBF5D-9647-6129-9D11-80B65663D574";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  12 2 0 12 2 0 12 2 0 12 2 
		0 12 2 0 12 2 0 12 2 0 12 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Rook|HitBox113|HitBox|HitBox";
	rename -uid "8020C117-6D49-6F5E-2AB4-8B9E06CB8375";
	setAttr ".rp" -type "double3" 16 2 0 ;
	setAttr ".sp" -type "double3" 16 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Rook|HitBox113|HitBox|HitBox|HitBox";
	rename -uid "2BB7D756-1F46-2243-33D4-19A4EEECB8FC";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  16 2 0 16 2 0 16 2 0 16 2 
		0 16 2 0 16 2 0 16 2 0 16 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Rook|HitBox113|HitBox|HitBox|HitBox";
	rename -uid "3F96DF96-9243-6BFB-B58F-AC9DC3D0390B";
	setAttr ".rp" -type "double3" 20 2 0 ;
	setAttr ".sp" -type "double3" 20 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Rook|HitBox113|HitBox|HitBox|HitBox|HitBox";
	rename -uid "76C614D7-3B47-0EBD-4826-FAB41EFDB4DE";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  20 2 0 20 2 0 20 2 0 20 2 
		0 20 2 0 20 2 0 20 2 0 20 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Rook|HitBox113|HitBox|HitBox|HitBox|HitBox";
	rename -uid "5FD50182-3942-9B14-EAFB-9B83E8141089";
	setAttr ".rp" -type "double3" 24 2 0 ;
	setAttr ".sp" -type "double3" 24 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Rook|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "857A97D6-2E41-48C6-7B30-1A90C4AD7B18";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  24 2 0 24 2 0 24 2 0 24 2 
		0 24 2 0 24 2 0 24 2 0 24 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Rook|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "725733A3-E54E-3C5D-F6E5-318FBC7A5B53";
	setAttr ".rp" -type "double3" 28 2 0 ;
	setAttr ".sp" -type "double3" 28 2 0 ;
createNode mesh -n "HitBoxShape" -p "|Rook|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBox|HitBox";
	rename -uid "CA56543A-9B46-6B0A-02C3-489406E1679A";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  28 2 0 28 2 0 28 2 0 28 2 
		0 28 2 0 28 2 0 28 2 0 28 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox71" -p "Rook";
	rename -uid "C14867CF-984D-B2FB-B18C-C28E33C0E732";
	setAttr ".rp" -type "double3" 0 2 4 ;
	setAttr ".sp" -type "double3" 0 2 4 ;
createNode mesh -n "HitBox71Shape" -p "|Rook|HitBox71";
	rename -uid "DEF76F69-BF45-AF81-A31F-46B4D37DB901";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 4 0 2 4 0 2 4 0 2 4 0 
		2 4 0 2 4 0 2 4 0 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox72" -p "|Rook|HitBox71";
	rename -uid "C7877C08-EE4F-04E0-1086-ED8C998B1802";
	setAttr ".rp" -type "double3" 0 2 8 ;
	setAttr ".sp" -type "double3" 0 2 8 ;
createNode mesh -n "HitBox72Shape" -p "|Rook|HitBox71|HitBox72";
	rename -uid "2B00D711-1D46-E7AB-3276-849DFB82628F";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 8 0 2 8 0 2 8 0 2 8 0 
		2 8 0 2 8 0 2 8 0 2 8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox73" -p "|Rook|HitBox71|HitBox72";
	rename -uid "ECD04A42-EB4B-76F5-52F8-438270859A58";
	setAttr ".rp" -type "double3" 0 2 12 ;
	setAttr ".sp" -type "double3" 0 2 12 ;
createNode mesh -n "HitBox73Shape" -p "|Rook|HitBox71|HitBox72|HitBox73";
	rename -uid "4F8F6283-0347-A307-4168-EDA8D8DFA863";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 12 0 2 12 0 2 12 0 2 
		12 0 2 12 0 2 12 0 2 12 0 2 12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox74" -p "|Rook|HitBox71|HitBox72|HitBox73";
	rename -uid "0A6AE26A-5A4B-BA6D-0791-11805A1FB982";
	setAttr ".rp" -type "double3" 0 2 16 ;
	setAttr ".sp" -type "double3" 0 2 16 ;
createNode mesh -n "HitBox74Shape" -p "|Rook|HitBox71|HitBox72|HitBox73|HitBox74";
	rename -uid "D888014D-6248-EB70-1B3E-7BBD8B2AC37C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 16 0 2 16 0 2 16 0 2 
		16 0 2 16 0 2 16 0 2 16 0 2 16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox75" -p "|Rook|HitBox71|HitBox72|HitBox73|HitBox74";
	rename -uid "A136BEFD-F742-7DB9-15EC-BF8BF846A562";
	setAttr ".rp" -type "double3" 0 2 20 ;
	setAttr ".sp" -type "double3" 0 2 20 ;
createNode mesh -n "HitBox75Shape" -p "|Rook|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75";
	rename -uid "38A3AF36-1B4D-2C28-6FAC-5D93BB05980D";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 20 0 2 20 0 2 20 0 2 
		20 0 2 20 0 2 20 0 2 20 0 2 20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox76" -p "|Rook|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75";
	rename -uid "B3DA40C3-C64A-FF52-0951-5AA51CEF5943";
	setAttr ".rp" -type "double3" 0 2 24 ;
	setAttr ".sp" -type "double3" 0 2 24 ;
createNode mesh -n "HitBox76Shape" -p "|Rook|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox76";
	rename -uid "4FC10D45-B14E-1653-868A-8DB9515B724A";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 24 0 2 24 0 2 24 0 2 
		24 0 2 24 0 2 24 0 2 24 0 2 24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox77" -p "|Rook|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox76";
	rename -uid "D2947381-1A45-514D-A636-C7AB1BF7A7C3";
	setAttr ".rp" -type "double3" 0 2 28 ;
	setAttr ".sp" -type "double3" 0 2 28 ;
createNode mesh -n "HitBox77Shape" -p "|Rook|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox76|HitBox77";
	rename -uid "1C308701-5243-7E00-8930-3883420844E5";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 28 0 2 28 0 2 28 0 2 
		28 0 2 28 0 2 28 0 2 28 0 2 28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox99" -p "Rook";
	rename -uid "21D1214D-DF4D-E58A-C0D8-55AB092D5C49";
	setAttr ".rp" -type "double3" 0 2 -4 ;
	setAttr ".sp" -type "double3" 0 2 -4 ;
createNode mesh -n "HitBox99Shape" -p "|Rook|HitBox99";
	rename -uid "606987B5-DE4E-CCE4-61F0-7296B6FACCD8";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -4 0 2 -4 0 2 -4 0 2 
		-4 0 2 -4 0 2 -4 0 2 -4 0 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox57" -p "|Rook|HitBox99";
	rename -uid "DFD332AD-5A46-416C-D35F-3382A7674CFB";
	setAttr ".rp" -type "double3" 0 2 -8 ;
	setAttr ".sp" -type "double3" 0 2 -8 ;
createNode mesh -n "HitBox57Shape" -p "|Rook|HitBox99|HitBox57";
	rename -uid "1415CD91-644C-2ABD-76ED-7B93E08AB323";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -8 0 2 -8 0 2 -8 0 2 
		-8 0 2 -8 0 2 -8 0 2 -8 0 2 -8;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox56" -p "|Rook|HitBox99|HitBox57";
	rename -uid "72D8D3ED-F149-B439-473C-7DBE640F0FC2";
	setAttr ".rp" -type "double3" 0 2 -12 ;
	setAttr ".sp" -type "double3" 0 2 -12 ;
createNode mesh -n "HitBox56Shape" -p "|Rook|HitBox99|HitBox57|HitBox56";
	rename -uid "C3D957C0-F742-B9FE-EF90-8BA429BFB66C";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -12 0 2 -12 0 2 -12 0 
		2 -12 0 2 -12 0 2 -12 0 2 -12 0 2 -12;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox102" -p "|Rook|HitBox99|HitBox57|HitBox56";
	rename -uid "6047B49E-D247-E0BC-0556-65B8DC8FCE8B";
	setAttr ".rp" -type "double3" 0 2 -16 ;
	setAttr ".sp" -type "double3" 0 2 -16 ;
createNode mesh -n "HitBox102Shape" -p "|Rook|HitBox99|HitBox57|HitBox56|HitBox102";
	rename -uid "DE349283-6D41-14ED-4FD3-5D9C00CFFD3D";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -16 0 2 -16 0 2 -16 0 
		2 -16 0 2 -16 0 2 -16 0 2 -16 0 2 -16;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Rook|HitBox99|HitBox57|HitBox56|HitBox102";
	rename -uid "747C6BA1-E941-5EFB-AB3E-96996D917DFD";
	setAttr ".rp" -type "double3" 0 2 -20 ;
	setAttr ".sp" -type "double3" 0 2 -20 ;
createNode mesh -n "HitBoxShape" -p "|Rook|HitBox99|HitBox57|HitBox56|HitBox102|HitBox";
	rename -uid "A8F31D5B-2447-EFDF-51C0-F3A37CFCD388";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -20 0 2 -20 0 2 -20 0 
		2 -20 0 2 -20 0 2 -20 0 2 -20 0 2 -20;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Rook|HitBox99|HitBox57|HitBox56|HitBox102|HitBox";
	rename -uid "7DCEDB90-5C49-E356-B185-6FAEE37B13A2";
	setAttr ".rp" -type "double3" 0 2 -24 ;
	setAttr ".sp" -type "double3" 0 2 -24 ;
createNode mesh -n "HitBoxShape" -p "|Rook|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBox";
	rename -uid "E66801D5-E541-EBDD-FE19-20916A6B272B";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -24 0 2 -24 0 2 -24 0 
		2 -24 0 2 -24 0 2 -24 0 2 -24 0 2 -24;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox" -p "|Rook|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBox";
	rename -uid "A33947C8-C447-7130-6F5C-B2A889AED214";
	setAttr ".rp" -type "double3" 0 2 -28 ;
	setAttr ".sp" -type "double3" 0 2 -28 ;
createNode mesh -n "HitBoxShape" -p "|Rook|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBox|HitBox";
	rename -uid "F19222A9-6041-5094-5026-29839B70E114";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -28 0 2 -28 0 2 -28 0 
		2 -28 0 2 -28 0 2 -28 0 2 -28 0 2 -28;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "King";
	rename -uid "54CC1D72-5B4C-EBEA-721B-4984934778BC";
	setAttr ".t" -type "double3" -52 0 -45 ;
createNode transform -n "HitBox51" -p "King";
	rename -uid "7520B20B-F446-2A71-2EF3-7F93FB4666BB";
	setAttr ".rp" -type "double3" -4 2 0 ;
	setAttr ".sp" -type "double3" -4 2 0 ;
createNode mesh -n "HitBox51Shape" -p "|King|HitBox51";
	rename -uid "F9C6089D-2347-0169-3749-81B4BE5CD3FB";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 0 -4 2 0 -4 2 0 -4 2 
		0 -4 2 0 -4 2 0 -4 2 0 -4 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox113" -p "King";
	rename -uid "F64D43C3-B24C-7603-A202-B285DB1C52EB";
	setAttr ".rp" -type "double3" 4 2 0 ;
	setAttr ".sp" -type "double3" 4 2 0 ;
createNode mesh -n "HitBox113Shape" -p "|King|HitBox113";
	rename -uid "8592E985-3C41-F616-F2C7-FB8D96E15953";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 0 4 2 0 4 2 0 4 2 0 4 
		2 0 4 2 0 4 2 0 4 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox71" -p "King";
	rename -uid "DD5F4484-1C40-9A78-0C0A-57AF5B6904E1";
	setAttr ".rp" -type "double3" 0 2 4 ;
	setAttr ".sp" -type "double3" 0 2 4 ;
createNode mesh -n "HitBox71Shape" -p "|King|HitBox71";
	rename -uid "5D6D2A9D-7E4E-82D8-F761-7392309C1226";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 4 0 2 4 0 2 4 0 2 4 0 
		2 4 0 2 4 0 2 4 0 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox78" -p "King";
	rename -uid "AC5E0421-104D-1A4B-91CF-7A8EE2B8B879";
	setAttr ".rp" -type "double3" 4 2 4 ;
	setAttr ".sp" -type "double3" 4 2 4 ;
createNode mesh -n "HitBox78Shape" -p "|King|HitBox78";
	rename -uid "0A169004-B74E-2F96-359A-D1888E7F2804";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 4 4 2 4 4 2 4 4 2 4 4 
		2 4 4 2 4 4 2 4 4 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox85" -p "King";
	rename -uid "D876F314-E445-F78A-36DC-B5B68303A687";
	setAttr ".rp" -type "double3" -4 2 4 ;
	setAttr ".sp" -type "double3" -4 2 4 ;
createNode mesh -n "HitBox85Shape" -p "|King|HitBox85";
	rename -uid "5139090B-7549-83A8-671B-DEA118CA5DF6";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 4 -4 2 4 -4 2 4 -4 2 
		4 -4 2 4 -4 2 4 -4 2 4 -4 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox92" -p "King";
	rename -uid "3EEBDCB1-194C-136A-E792-658F618153E0";
	setAttr ".rp" -type "double3" -4 2 -4 ;
	setAttr ".sp" -type "double3" -4 2 -4 ;
createNode mesh -n "HitBox92Shape" -p "|King|HitBox92";
	rename -uid "89C85BDC-DA4D-400F-C800-EF8C27906030";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 -4 -4 2 -4 -4 2 -4 -4 
		2 -4 -4 2 -4 -4 2 -4 -4 2 -4 -4 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox99" -p "King";
	rename -uid "B8703AE0-BF42-7468-3E2D-E38E8BBAD172";
	setAttr ".rp" -type "double3" 0 2 -4 ;
	setAttr ".sp" -type "double3" 0 2 -4 ;
createNode mesh -n "HitBox99Shape" -p "|King|HitBox99";
	rename -uid "B7862815-9A46-705D-AFA9-42BD998E46A2";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  0 2 -4 0 2 -4 0 2 -4 0 2 
		-4 0 2 -4 0 2 -4 0 2 -4 0 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox0" -p "King";
	rename -uid "B0840EDF-0D40-CF06-EFC1-28AF3702CABE";
	setAttr ".rp" -type "double3" 4 2 -4 ;
	setAttr ".sp" -type "double3" 4 2 -4 ;
createNode mesh -n "HitBox0Shape" -p "|King|HitBox0";
	rename -uid "CE23A575-0045-127F-49F4-5A889BA90037";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  4 2 -4 4 2 -4 4 2 -4 4 2 
		-4 4 2 -4 4 2 -4 4 2 -4 4 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "Pawn";
	rename -uid "B3DD502E-314A-F0C0-E7CE-1896CAD90552";
	setAttr ".t" -type "double3" -49 0 0 ;
createNode transform -n "HitBox51" -p "Pawn";
	rename -uid "C2B5C4FD-DA48-0C56-F59B-83B917E059EF";
	setAttr ".rp" -type "double3" -4 2 0 ;
	setAttr ".sp" -type "double3" -4 2 0 ;
createNode mesh -n "HitBox51Shape" -p "|Pawn|HitBox51";
	rename -uid "95722BA2-4648-C63B-89D8-00BDC78D9B8E";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 0 -4 2 0 -4 2 0 -4 2 
		0 -4 2 0 -4 2 0 -4 2 0 -4 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox43" -p "|Pawn|HitBox51";
	rename -uid "564CE1BC-E248-4840-73E9-ECB4FE814767";
	setAttr ".rp" -type "double3" -8 2 0 ;
	setAttr ".sp" -type "double3" -8 2 0 ;
createNode mesh -n "HitBox43Shape" -p "|Pawn|HitBox51|HitBox43";
	rename -uid "2A59096D-6A4A-C9A9-86CB-B8B48B22025A";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -8 2 0 -8 2 0 -8 2 0 -8 2 
		0 -8 2 0 -8 2 0 -8 2 0 -8 2 0;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox85" -p "Pawn";
	rename -uid "D5D6CA9F-664E-71EE-EDD0-43A9A3D89044";
	setAttr ".rp" -type "double3" -4 2 4 ;
	setAttr ".sp" -type "double3" -4 2 4 ;
createNode mesh -n "HitBox85Shape" -p "|Pawn|HitBox85";
	rename -uid "83408678-3C41-ECD2-3789-A5A354FE153B";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 4 -4 2 4 -4 2 4 -4 2 
		4 -4 2 4 -4 2 4 -4 2 4 -4 2 4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "HitBox92" -p "Pawn";
	rename -uid "672F4CDE-D848-BE57-8C24-CDB23B4C1702";
	setAttr ".rp" -type "double3" -4 2 -4 ;
	setAttr ".sp" -type "double3" -4 2 -4 ;
createNode mesh -n "HitBox92Shape" -p "|Pawn|HitBox92";
	rename -uid "6D0423DC-1A4D-B453-C4BB-8697BFAC103E";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 14 ".uvst[0].uvsp[0:13]" -type "float2" 0.375 0 0.625 0 0.375
		 0.25 0.625 0.25 0.375 0.5 0.625 0.5 0.375 0.75 0.625 0.75 0.375 1 0.625 1 0.875 0
		 0.875 0.25 0.125 0 0.125 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 8 ".pt[0:7]" -type "float3"  -4 2 -4 -4 2 -4 -4 2 -4 -4 
		2 -4 -4 2 -4 -4 2 -4 -4 2 -4 -4 2 -4;
	setAttr -s 8 ".vt[0:7]"  -1.95000005 -1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005
		 -1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 1.95000005 -1.95000005 1.95000005 -1.95000005
		 1.95000005 1.95000005 -1.95000005 -1.95000005 -1.95000005 -1.95000005 1.95000005 -1.95000005 -1.95000005;
	setAttr -s 12 ".ed[0:11]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0;
	setAttr -s 6 -ch 24 ".fc[0:5]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 3 2
		f 4 1 7 -3 -7
		mu 0 4 2 3 5 4
		f 4 2 9 -4 -9
		mu 0 4 4 5 7 6
		f 4 3 11 -1 -11
		mu 0 4 6 7 9 8
		f 4 -12 -10 -8 -6
		mu 0 4 1 10 11 3
		f 4 10 4 6 8
		mu 0 4 12 0 2 13;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
	setAttr ".dr" 1;
createNode transform -n "ChessBoard";
	rename -uid "01422AC3-5B4E-3286-96C5-BDB91C37C996";
createNode mesh -n "ChessBoardShape" -p "ChessBoard";
	rename -uid "64137324-094B-6FBE-23D3-B583F8F4A052";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode transform -n "BasePiece";
	rename -uid "83381321-B740-C8D1-BD87-CD824F6A2B1D";
createNode mesh -n "BasePieceShape" -p "BasePiece";
	rename -uid "C6DB3BFB-104D-A626-9FD4-AC9274891ACB";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode lightLinker -s -n "lightLinker1";
	rename -uid "28348BD1-A646-C160-E753-108E3C6402F9";
	setAttr -s 4 ".lnk";
	setAttr -s 4 ".slnk";
createNode displayLayerManager -n "layerManager";
	rename -uid "C2E72391-FB42-8C1C-3734-56AD8E694CFC";
createNode displayLayer -n "defaultLayer";
	rename -uid "CF01ECFD-864C-D9C0-C651-949B1A92004D";
createNode renderLayerManager -n "renderLayerManager";
	rename -uid "7321916D-A649-6BED-F672-26AA0AF96A4E";
createNode renderLayer -n "defaultRenderLayer";
	rename -uid "1683F53C-4944-CCD5-3E62-64A1D392D526";
	setAttr ".g" yes;
createNode shapeEditorManager -n "shapeEditorManager";
	rename -uid "6DF169A1-FC42-D35D-7FB0-E6B0E78DD7AC";
createNode poseInterpolatorManager -n "poseInterpolatorManager";
	rename -uid "3DBFB350-574D-1196-3494-10A530754245";
createNode script -n "uiConfigurationScriptNode";
	rename -uid "AB4684F4-A341-5815-F7BB-A9B60BE85F85";
	setAttr ".b" -type "string" (
		"// Maya Mel UI Configuration File.\n//\n//  This script is machine generated.  Edit at your own risk.\n//\n//\n\nglobal string $gMainPane;\nif (`paneLayout -exists $gMainPane`) {\n\n\tglobal int $gUseScenePanelConfig;\n\tint    $useSceneConfig = $gUseScenePanelConfig;\n\tint    $menusOkayInPanels = `optionVar -q allowMenusInPanels`;\tint    $nVisPanes = `paneLayout -q -nvp $gMainPane`;\n\tint    $nPanes = 0;\n\tstring $editorName;\n\tstring $panelName;\n\tstring $itemFilterName;\n\tstring $panelConfig;\n\n\t//\n\t//  get current state of the UI\n\t//\n\tsceneUIReplacement -update $gMainPane;\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Top View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Top View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"top\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n"
		+ "            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n"
		+ "            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n"
		+ "            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 525\n            -height 255\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Side View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Side View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"side\" \n"
		+ "            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n"
		+ "            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n"
		+ "            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 524\n            -height 254\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Front View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Front View\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"front\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n"
		+ "            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n"
		+ "            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 525\n            -height 254\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Persp View\")) `;\n\tif (\"\" != $panelName) {\n"
		+ "\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Persp View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"persp\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 1\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n"
		+ "            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n"
		+ "            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 859\n            -height 534\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n"
		+ "\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"outlinerPanel\" (localizedPanelLabel(\"ToggledOutliner\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\toutlinerPanel -edit -l (localizedPanelLabel(\"ToggledOutliner\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        outlinerEditor -e \n            -showShapes 0\n            -showAssignedMaterials 0\n            -showTimeEditor 1\n            -showReferenceNodes 0\n            -showReferenceMembers 0\n            -showAttributes 0\n            -showConnected 0\n            -showAnimCurvesOnly 0\n            -showMuteInfo 0\n            -organizeByLayer 1\n            -showAnimLayerWeight 1\n            -autoExpandLayers 1\n            -autoExpand 0\n            -showDagOnly 1\n            -showAssets 1\n            -showContainedOnly 1\n            -showPublishedAsConnected 0\n            -showContainerContents 1\n            -ignoreDagHierarchy 0\n            -expandConnections 0\n            -showUpstreamCurves 1\n            -showUnitlessCurves 1\n"
		+ "            -showCompounds 1\n            -showLeafs 1\n            -showNumericAttrsOnly 0\n            -highlightActive 1\n            -autoSelectNewObjects 0\n            -doNotSelectNewObjects 0\n            -dropIsParent 1\n            -transmitFilters 0\n            -setFilter \"0\" \n            -showSetMembers 1\n            -allowMultiSelection 1\n            -alwaysToggleSelect 0\n            -directSelect 0\n            -isSet 0\n            -isSetMember 0\n            -displayMode \"DAG\" \n            -expandObjects 0\n            -setsIgnoreFilters 1\n            -containersIgnoreFilters 0\n            -editAttrName 0\n            -showAttrValues 0\n            -highlightSecondary 0\n            -showUVAttrsOnly 0\n            -showTextureNodesOnly 0\n            -attrAlphaOrder \"default\" \n            -animLayerFilterOptions \"allAffecting\" \n            -sortOrder \"none\" \n            -longNames 0\n            -niceNames 1\n            -showNamespace 1\n            -showPinIcons 0\n            -mapMotionTrails 0\n            -ignoreHiddenAttribute 0\n"
		+ "            -ignoreOutlinerColor 0\n            -renderFilterVisible 0\n            -renderFilterIndex 0\n            -selectionOrder \"chronological\" \n            -expandAttribute 0\n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"outlinerPanel\" (localizedPanelLabel(\"Outliner\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\toutlinerPanel -edit -l (localizedPanelLabel(\"Outliner\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        outlinerEditor -e \n            -showShapes 0\n            -showAssignedMaterials 0\n            -showTimeEditor 1\n            -showReferenceNodes 0\n            -showReferenceMembers 0\n            -showAttributes 0\n            -showConnected 0\n            -showAnimCurvesOnly 0\n            -showMuteInfo 0\n            -organizeByLayer 1\n            -showAnimLayerWeight 1\n            -autoExpandLayers 1\n            -autoExpand 0\n            -showDagOnly 1\n"
		+ "            -showAssets 1\n            -showContainedOnly 1\n            -showPublishedAsConnected 0\n            -showContainerContents 1\n            -ignoreDagHierarchy 0\n            -expandConnections 0\n            -showUpstreamCurves 1\n            -showUnitlessCurves 1\n            -showCompounds 1\n            -showLeafs 1\n            -showNumericAttrsOnly 0\n            -highlightActive 1\n            -autoSelectNewObjects 0\n            -doNotSelectNewObjects 0\n            -dropIsParent 1\n            -transmitFilters 0\n            -setFilter \"defaultSetFilter\" \n            -showSetMembers 1\n            -allowMultiSelection 1\n            -alwaysToggleSelect 0\n            -directSelect 0\n            -displayMode \"DAG\" \n            -expandObjects 0\n            -setsIgnoreFilters 1\n            -containersIgnoreFilters 0\n            -editAttrName 0\n            -showAttrValues 0\n            -highlightSecondary 0\n            -showUVAttrsOnly 0\n            -showTextureNodesOnly 0\n            -attrAlphaOrder \"default\" \n"
		+ "            -animLayerFilterOptions \"allAffecting\" \n            -sortOrder \"none\" \n            -longNames 0\n            -niceNames 1\n            -showNamespace 1\n            -showPinIcons 0\n            -mapMotionTrails 0\n            -ignoreHiddenAttribute 0\n            -ignoreOutlinerColor 0\n            -renderFilterVisible 0\n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"graphEditor\" (localizedPanelLabel(\"Graph Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Graph Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAssignedMaterials 0\n                -showTimeEditor 1\n                -showReferenceNodes 0\n                -showReferenceMembers 0\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n"
		+ "                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 1\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 0\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 1\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 1\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n"
		+ "                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 1\n                -mapMotionTrails 1\n                -ignoreHiddenAttribute 0\n                -ignoreOutlinerColor 0\n                -renderFilterVisible 0\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"GraphEd\");\n            animCurveEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 1\n                -displayInfinities 0\n                -displayValues 0\n                -autoFit 1\n                -snapTime \"integer\" \n"
		+ "                -snapValue \"none\" \n                -showResults \"off\" \n                -showBufferCurves \"off\" \n                -smoothness \"fine\" \n                -resultSamples 1\n                -resultScreenSamples 0\n                -resultUpdate \"delayed\" \n                -showUpstreamCurves 1\n                -showCurveNames 0\n                -showActiveCurveNames 0\n                -stackedCurves 0\n                -stackedCurvesMin -1\n                -stackedCurvesMax 1\n                -stackedCurvesSpace 0.2\n                -displayNormalized 0\n                -preSelectionHighlight 0\n                -constrainDrag 0\n                -classicMode 1\n                -valueLinesToggle 1\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dopeSheetPanel\" (localizedPanelLabel(\"Dope Sheet\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dope Sheet\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAssignedMaterials 0\n                -showTimeEditor 1\n                -showReferenceNodes 0\n                -showReferenceMembers 0\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 0\n                -showCompounds 1\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 0\n"
		+ "                -doNotSelectNewObjects 1\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 1\n                -ignoreHiddenAttribute 0\n                -ignoreOutlinerColor 0\n                -renderFilterVisible 0\n                $editorName;\n"
		+ "\n\t\t\t$editorName = ($panelName+\"DopeSheetEd\");\n            dopeSheetEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -displayValues 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -outliner \"dopeSheetPanel1OutlineEd\" \n                -showSummary 1\n                -showScene 0\n                -hierarchyBelow 0\n                -showTicks 1\n                -selectionWindow 0 0 0 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"timeEditorPanel\" (localizedPanelLabel(\"Time Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Time Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n"
		+ "\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"clipEditorPanel\" (localizedPanelLabel(\"Trax Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Trax Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = clipEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -displayValues 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -initialized 0\n                -manageSequencer 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"sequenceEditorPanel\" (localizedPanelLabel(\"Camera Sequencer\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n"
		+ "\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Camera Sequencer\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = sequenceEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -displayValues 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -initialized 0\n                -manageSequencer 1 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperGraphPanel\" (localizedPanelLabel(\"Hypergraph Hierarchy\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypergraph Hierarchy\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"HyperGraphEd\");\n            hyperGraph -e \n"
		+ "                -graphLayoutStyle \"hierarchicalLayout\" \n                -orientation \"horiz\" \n                -mergeConnections 0\n                -zoom 1\n                -animateTransition 0\n                -showRelationships 1\n                -showShapes 0\n                -showDeformers 0\n                -showExpressions 0\n                -showConstraints 0\n                -showConnectionFromSelected 0\n                -showConnectionToSelected 0\n                -showConstraintLabels 0\n                -showUnderworld 0\n                -showInvisible 0\n                -transitionFrames 1\n                -opaqueContainers 0\n                -freeform 0\n                -imagePosition 0 0 \n                -imageScale 1\n                -imageEnabled 0\n                -graphType \"DAG\" \n                -heatMapDisplay 0\n                -updateSelection 1\n                -updateNodeAdded 1\n                -useDrawOverrideColor 0\n                -limitGraphTraversal -1\n                -range 0 0 \n                -iconSize \"smallIcons\" \n"
		+ "                -showCachedConnections 0\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperShadePanel\" (localizedPanelLabel(\"Hypershade\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypershade\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"visorPanel\" (localizedPanelLabel(\"Visor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Visor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"nodeEditorPanel\" (localizedPanelLabel(\"Node Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Node Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\n\t\t\t$editorName = ($panelName+\"NodeEditorEd\");\n            nodeEditor -e \n                -allAttributes 0\n                -allNodes 0\n                -autoSizeNodes 1\n                -consistentNameSize 1\n                -createNodeCommand \"nodeEdCreateNodeCommand\" \n                -defaultPinnedState 0\n                -additiveGraphingMode 0\n                -settingsChangedCallback \"nodeEdSyncControls\" \n                -traversalDepthLimit -1\n                -keyPressCommand \"nodeEdKeyPressCommand\" \n                -nodeTitleMode \"name\" \n                -gridSnap 0\n                -gridVisibility 1\n                -popupMenuScript \"nodeEdBuildPanelMenus\" \n                -showNamespace 1\n                -showShapes 1\n                -showSGShapes 0\n                -showTransforms 1\n                -useAssets 1\n                -syncedSelection 1\n                -extendToShapes 1\n                -activeTab -1\n                -editorMode \"default\" \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n"
		+ "\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"createNodePanel\" (localizedPanelLabel(\"Create Node\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Create Node\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"polyTexturePlacementPanel\" (localizedPanelLabel(\"UV Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"UV Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"renderWindowPanel\" (localizedPanelLabel(\"Render View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Render View\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n"
		+ "\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"shapePanel\" (localizedPanelLabel(\"Shape Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tshapePanel -edit -l (localizedPanelLabel(\"Shape Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"posePanel\" (localizedPanelLabel(\"Pose Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tposePanel -edit -l (localizedPanelLabel(\"Pose Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynRelEdPanel\" (localizedPanelLabel(\"Dynamic Relationships\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dynamic Relationships\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n"
		+ "\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"relationshipPanel\" (localizedPanelLabel(\"Relationship Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Relationship Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"referenceEditorPanel\" (localizedPanelLabel(\"Reference Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Reference Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"componentEditorPanel\" (localizedPanelLabel(\"Component Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Component Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynPaintScriptedPanelType\" (localizedPanelLabel(\"Paint Effects\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Paint Effects\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"scriptEditorPanel\" (localizedPanelLabel(\"Script Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Script Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"profilerPanel\" (localizedPanelLabel(\"Profiler Tool\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Profiler Tool\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"contentBrowserPanel\" (localizedPanelLabel(\"Content Browser\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Content Browser\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\tif ($useSceneConfig) {\n        string $configName = `getPanel -cwl (localizedPanelLabel(\"Current Layout\"))`;\n        if (\"\" != $configName) {\n\t\t\tpanelConfiguration -edit -label (localizedPanelLabel(\"Current Layout\")) \n\t\t\t\t-userCreated false\n\t\t\t\t-defaultImage \"vacantCell.xP:/\"\n\t\t\t\t-image \"\"\n\t\t\t\t-sc false\n\t\t\t\t-configString \"global string $gMainPane; paneLayout -e -cn \\\"single\\\" -ps 1 100 100 $gMainPane;\"\n\t\t\t\t-removeAllPanels\n\t\t\t\t-ap false\n\t\t\t\t\t(localizedPanelLabel(\"Persp View\")) \n\t\t\t\t\t\"modelPanel\"\n"
		+ "\t\t\t\t\t\"$panelName = `modelPanel -unParent -l (localizedPanelLabel(\\\"Persp View\\\")) -mbv $menusOkayInPanels `;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera persp` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"smoothShaded\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 1\\n    -headsUpDisplay 1\\n    -holdOuts 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 0\\n    -backfaceCulling 0\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 0\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 16384\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -depthOfFieldPreview 1\\n    -maxConstantTransparency 1\\n    -rendererName \\\"vp2Renderer\\\" \\n    -objectFilterShowInHUD 1\\n    -isFiltered 0\\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"frontAndBackCull\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 1\\n    -cameras 1\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 1\\n    -imagePlane 1\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -particleInstancers 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -pluginShapes 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -clipGhosts 1\\n    -greasePencils 1\\n    -shadows 0\\n    -captureSequenceNumber -1\\n    -width 859\\n    -height 534\\n    -sceneRenderFilter 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName\"\n"
		+ "\t\t\t\t\t\"modelPanel -edit -l (localizedPanelLabel(\\\"Persp View\\\")) -mbv $menusOkayInPanels  $panelName;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera persp` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"smoothShaded\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 1\\n    -headsUpDisplay 1\\n    -holdOuts 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 0\\n    -backfaceCulling 0\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 0\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 16384\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -depthOfFieldPreview 1\\n    -maxConstantTransparency 1\\n    -rendererName \\\"vp2Renderer\\\" \\n    -objectFilterShowInHUD 1\\n    -isFiltered 0\\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"frontAndBackCull\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 1\\n    -cameras 1\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 1\\n    -imagePlane 1\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -particleInstancers 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -pluginShapes 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -clipGhosts 1\\n    -greasePencils 1\\n    -shadows 0\\n    -captureSequenceNumber -1\\n    -width 859\\n    -height 534\\n    -sceneRenderFilter 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName\"\n"
		+ "\t\t\t\t$configName;\n\n            setNamedPanelLayout (localizedPanelLabel(\"Current Layout\"));\n        }\n\n        panelHistory -e -clear mainPanelHistory;\n        sceneUIReplacement -clear;\n\t}\n\n\ngrid -spacing 5 -size 12 -divisions 5 -displayAxes yes -displayGridLines yes -displayDivisionLines yes -displayPerspectiveLabels no -displayOrthographicLabels no -displayAxesBold yes -perspectiveLabelPosition axis -orthographicLabelPosition edge;\nviewManip -drawCompass 0 -compassAngle 0 -frontParameters \"\" -homeParameters \"\" -selectionLockParameters \"\";\n}\n");
	setAttr ".st" 3;
createNode script -n "sceneConfigurationScriptNode";
	rename -uid "ED2CAB79-0F45-5269-E935-81B07D4AA7FD";
	setAttr ".b" -type "string" "playbackOptions -min 1 -max 120 -ast 1 -aet 200 ";
	setAttr ".st" 6;
createNode polyCube -n "polyCube3";
	rename -uid "04CFFAAA-8E49-E1BA-E381-D3B45332A972";
	setAttr ".w" 3.9;
	setAttr ".h" 3.9;
	setAttr ".d" 3.9;
	setAttr ".cuv" 4;
createNode polyPlane -n "polyPlane1";
	rename -uid "84B90EF9-1B44-A564-6797-32ADF39B120E";
	setAttr ".w" 32;
	setAttr ".h" 32;
	setAttr ".sw" 8;
	setAttr ".sh" 8;
	setAttr ".cuv" 2;
createNode polyExtrudeFace -n "polyExtrudeFace1";
	rename -uid "39AD5A3B-994B-9F91-1A70-26BB0A80B20D";
	setAttr ".ics" -type "componentList" 1 "f[0:63]";
	setAttr ".ix" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1;
	setAttr ".ws" yes;
	setAttr ".rs" 274726821;
	setAttr ".off" 0.05000000074505806;
	setAttr ".kft" no;
	setAttr ".c[0]"  0 1 1;
	setAttr ".tk" 0.10000000149011612;
	setAttr ".cbn" -type "double3" -16 -3.5527136788005009e-15 -16 ;
	setAttr ".cbx" -type "double3" 16 3.5527136788005009e-15 16 ;
createNode polyCube -n "polyCube4";
	rename -uid "03FCD8B9-D543-6729-4716-0FAC445D0446";
	setAttr ".w" 1.6;
	setAttr ".h" 4;
	setAttr ".d" 1.6;
	setAttr ".cuv" 4;
createNode transformGeometry -n "transformGeometry1";
	rename -uid "E04C21F5-4A43-2F0B-8C44-E3B12B9B642F";
	setAttr ".txf" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 -0.099999994039535522 0 1;
createNode transformGeometry -n "transformGeometry2";
	rename -uid "F08DD58C-294E-EBF2-C87D-4A86D453CA98";
	setAttr ".txf" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 2 0 1;
createNode lambert -n "Collider";
	rename -uid "D9AED989-9B4A-619A-0394-D4902B6D40C8";
	setAttr ".c" -type "float3" 0 1 1 ;
	setAttr ".it" -type "float3" 0.82580644 0.82580644 0.82580644 ;
createNode shadingEngine -n "lambert2SG";
	rename -uid "DC2A6175-C544-8541-9F48-1D82A1F05E7B";
	setAttr ".ihi" 0;
	setAttr -s 132 ".dsm";
	setAttr ".ro" yes;
createNode materialInfo -n "materialInfo1";
	rename -uid "1F8A916A-3A44-4241-4E9C-9AB5A5636B50";
createNode lambert -n "BoardArea";
	rename -uid "72D999DF-7549-5A98-0B45-A68FC65B3B4A";
	setAttr ".c" -type "float3" 1 1 0 ;
	setAttr ".it" -type "float3" 0.81935483 0.81935483 0.81935483 ;
createNode shadingEngine -n "lambert3SG";
	rename -uid "728C64BF-A84D-2A95-A5C2-A69EB7CDE8B4";
	setAttr ".ihi" 0;
	setAttr -s 64 ".dsm";
	setAttr ".ro" yes;
createNode materialInfo -n "materialInfo2";
	rename -uid "9A78E174-0B44-11A3-7296-B6B257855F66";
select -ne :time1;
	setAttr ".o" 15;
	setAttr ".unw" 15;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 22 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surface" "Particles" "Particle Instance" "Fluids" "Strokes" "Image Planes" "UI" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Hair Systems" "Follicles" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 22 0 1 1 1 1 1
		 1 1 1 0 0 0 0 0 0 0 0 0
		 0 0 0 0 ;
	setAttr ".fprt" yes;
select -ne :renderPartition;
	setAttr -s 4 ".st";
select -ne :renderGlobalsList1;
select -ne :defaultShaderList1;
	setAttr -s 6 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderingList1;
select -ne :initialShadingGroup;
	setAttr -s 2 ".dsm";
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
select -ne :ikSystem;
	setAttr -s 4 ".sol";
connectAttr "polyCube3.out" "|ChessHitGrid|HitBox|HitBoxShape.i";
connectAttr "transformGeometry1.og" "ChessBoardShape.i";
connectAttr "transformGeometry2.og" "BasePieceShape.i";
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "lambert2SG.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "lambert3SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "lambert2SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "lambert3SG.message" ":defaultLightSet.message";
connectAttr "layerManager.dli[0]" "defaultLayer.id";
connectAttr "renderLayerManager.rlmi[0]" "defaultRenderLayer.rlid";
connectAttr "polyPlane1.out" "polyExtrudeFace1.ip";
connectAttr "ChessBoardShape.wm" "polyExtrudeFace1.mp";
connectAttr "polyExtrudeFace1.out" "transformGeometry1.ig";
connectAttr "polyCube4.out" "transformGeometry2.ig";
connectAttr "Collider.oc" "lambert2SG.ss";
connectAttr "|Knight|HitBox35|HitBox35Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Knight|HitBox60|HitBox60Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Knight|HitBox28|HitBox28Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Knight|HitBox51|HitBox51Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Knight|HitBox24|HitBox24Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Knight|HitBox38|HitBox38Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Knight|HitBox54|HitBox54Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Knight|HitBox56|HitBox56Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Rook|HitBox113|HitBox113Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Rook|HitBox113|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm" -na
		;
connectAttr "|Rook|HitBox113|HitBox|HitBoxShape.iog" "lambert2SG.dsm" -na;
connectAttr "|Rook|HitBox71|HitBox71Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Rook|HitBox71|HitBox72|HitBox72Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Rook|HitBox71|HitBox72|HitBox73|HitBox73Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox71|HitBox72|HitBox73|HitBox74|HitBox74Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox75Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox51|HitBox51Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Rook|HitBox51|HitBox43|HitBox43Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Rook|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox11|HitBox7|HitBox7Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox11|HitBox11Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox19Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox51|HitBox43|HitBox35|HitBox27|HitBox27Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox51|HitBox43|HitBox35|HitBox35Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox113|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox99|HitBox57|HitBox57Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Rook|HitBox99|HitBox99Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Rook|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox99|HitBox57|HitBox56|HitBox102|HitBox102Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox99|HitBox57|HitBox56|HitBox56Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox76|HitBox77|HitBox77Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Rook|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox76|HitBox76Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|King|HitBox0|HitBox0Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|King|HitBox92|HitBox92Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|King|HitBox99|HitBox99Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|King|HitBox78|HitBox78Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|King|HitBox51|HitBox51Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Pawn|HitBox51|HitBox51Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Pawn|HitBox85|HitBox85Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Pawn|HitBox51|HitBox43|HitBox43Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox92|HitBox92Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|King|HitBox71|HitBox71Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox83|HitBox84|HitBox84Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox83|HitBox83Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox82Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox78|HitBox79|HitBox80|HitBox81|HitBox81Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox78|HitBox79|HitBox80|HitBox80Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox78|HitBox79|HitBox79Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox78|HitBox78Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox71|HitBox72|HitBox72Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox71|HitBox71Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox71|HitBox72|HitBox73|HitBox73Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox71|HitBox72|HitBox73|HitBox74|HitBox74Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox90|HitBox91|HitBox91Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox90|HitBox90Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox89Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox85|HitBox86|HitBox87|HitBox88|HitBox88Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox85|HitBox86|HitBox87|HitBox87Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox85|HitBox86|HitBox86Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox85|HitBox85Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox76|HitBox77|HitBox77Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox76|HitBox76Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox71|HitBox72|HitBox73|HitBox74|HitBox75|HitBox75Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Pawn|HitBox92|HitBox92Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox113|HitBox113Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox96Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox92|HitBox93|HitBox94|HitBox95|HitBox95Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox92|HitBox93|HitBox94|HitBox94Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox92|HitBox93|HitBox93Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox113|HitBox|HitBoxShape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox113|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox113|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox113|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm" -na
		;
connectAttr "|King|HitBox85|HitBox85Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox99|HitBox57|HitBox56|HitBox56Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox99|HitBox57|HitBox57Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox99|HitBox99Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox0|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox0|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox0|HitBox|HitBoxShape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox0|HitBox0Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox97|HitBox97Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox97|HitBox98|HitBox98Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox11|HitBox7|HitBox7Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox11|HitBox11Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox51|HitBox43|HitBox35|HitBox27|HitBox19|HitBox19Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox51|HitBox43|HitBox35|HitBox27|HitBox27Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox51|HitBox43|HitBox35|HitBox35Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox51|HitBox43|HitBox43Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox51|HitBox51Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Queen|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox99|HitBox57|HitBox56|HitBox102|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Queen|HitBox99|HitBox57|HitBox56|HitBox102|HitBox102Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|King|HitBox113|HitBox113Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Bishop|HitBox0|HitBox0Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Bishop|HitBox0|HitBox|HitBoxShape.iog" "lambert2SG.dsm" -na;
connectAttr "|Bishop|HitBox0|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm" -na
		;
connectAttr "|Bishop|HitBox0|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox0|HitBox|HitBox|HitBox|HitBox|HitBox|HitBoxShape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox92|HitBox92Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Bishop|HitBox92|HitBox93|HitBox93Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Bishop|HitBox92|HitBox93|HitBox94|HitBox94Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95|HitBox95Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox96Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox97|HitBox97Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox92|HitBox93|HitBox94|HitBox95|HitBox96|HitBox97|HitBox98|HitBox98Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox78|HitBox78Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Bishop|HitBox78|HitBox79|HitBox79Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Bishop|HitBox78|HitBox79|HitBox80|HitBox80Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81|HitBox81Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox82Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox83|HitBox83Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox78|HitBox79|HitBox80|HitBox81|HitBox82|HitBox83|HitBox84|HitBox84Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox85|HitBox85Shape.iog" "lambert2SG.dsm" -na;
connectAttr "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox90|HitBox91|HitBox91Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox90|HitBox90Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88|HitBox89|HitBox89Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox85|HitBox86|HitBox87|HitBox88|HitBox88Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox85|HitBox86|HitBox87|HitBox87Shape.iog" "lambert2SG.dsm"
		 -na;
connectAttr "|Bishop|HitBox85|HitBox86|HitBox86Shape.iog" "lambert2SG.dsm" -na;
connectAttr "lambert2SG.msg" "materialInfo1.sg";
connectAttr "Collider.msg" "materialInfo1.m";
connectAttr "BoardArea.oc" "lambert3SG.ss";
connectAttr "|ChessHitGrid|HitBox|HitBoxShape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox4Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox5Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox58Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox57|HitBox57Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox56|HitBox56Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox15Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox9Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox63Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox34Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox35|HitBox35Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox32Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox13Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox12Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox1Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox17Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox3Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox2Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox51|HitBox51Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox52Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox50Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox14Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox8Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox11|HitBox11Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox19|HitBox19Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox23Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox29Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox31Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox10Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox39Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox16Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox30Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox18Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox22Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox38|HitBox38Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox40Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox21Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox20Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox45Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox44Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox48Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox49Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox53Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox47Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox46Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox55Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox60|HitBox60Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox54|HitBox54Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox41Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox42Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox43|HitBox43Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox59Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox27|HitBox27Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox26Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox28|HitBox28Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox24|HitBox24Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox25Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox36Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox37Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox6Shape.iog" "lambert3SG.dsm" -na;
connectAttr "|ChessHitGrid|HitBox7|HitBox7Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox33Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox61Shape.iog" "lambert3SG.dsm" -na;
connectAttr "HitBox62Shape.iog" "lambert3SG.dsm" -na;
connectAttr "lambert3SG.msg" "materialInfo2.sg";
connectAttr "BoardArea.msg" "materialInfo2.m";
connectAttr "lambert2SG.pa" ":renderPartition.st" -na;
connectAttr "lambert3SG.pa" ":renderPartition.st" -na;
connectAttr "Collider.msg" ":defaultShaderList1.s" -na;
connectAttr "BoardArea.msg" ":defaultShaderList1.s" -na;
connectAttr "defaultRenderLayer.msg" ":defaultRenderingList1.r" -na;
connectAttr "ChessBoardShape.iog" ":initialShadingGroup.dsm" -na;
connectAttr "BasePieceShape.iog" ":initialShadingGroup.dsm" -na;
// End of Board.ma
