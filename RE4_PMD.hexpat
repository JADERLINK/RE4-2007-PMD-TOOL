//------------------------------------------------
//--- ImHex v1.30.1 Pattern Template
//
//  File: RE4_PMD Parser
//   Authors: JADERLINK
//   Version: 1.1
// File Mask: *.pmd
//  ID Bytes: 
//   History: 
//   1.0   2023-09-16  initial version
//   1.1   2023-10-05  initial release
//------------------------------------------------

#pragma endian little
#pragma array_limit 2147483647
#pragma pattern_limit 2147483647

struct Material{
	float data[17];
	s32 textureEnable;
	s32 textureNameLenght;
	char TextureName[textureNameLenght];
};

struct PMDnodeBone{
	s32 boneId;
	float unknown[13];
	float x;
	float y;
	float z;
	float w;
};


struct PMDvertex{
	float x;
	float y;
	float z;
	float w0;
	float w1;
	float i0;
	float i1;
	float nx;
	float ny;
	float nz;
	float tu;
	float tv;
	float r;
	float g;
	float b;
	float a;
};

struct PMDmesh
{
	s32 fix40000000;
	if(fix40000000 == 64){
	
	u32 indexAmount;
	u16 Order[indexAmount];

	u32 vertexAmount;
	PMDvertex vertex[vertexAmount];
	}
};

struct TextureIndex
{
	s32 textureIndex;
};

struct PMDnode {
	s32 SkeletonIndex;
	
	padding[32];
	
	u32 textureIndex_lenght;
	TextureIndex textureIndex[textureIndex_lenght];

	padding[4];
	
	u32 mesh_lenght;
	PMDmesh mesh[mesh_lenght];

	u32 num_bones;
	PMDnodeBone nodeBone[num_bones];
};


struct SkeletonAndParent {
	s32 Parent;
	float Skeleton[26];
};

struct GroupMeshName
{
	u32 namelen;
	char name[namelen];
	u32 groupId;
};

struct SkeletonBoneName
{
	u32 namelen;
	char name[namelen];
	u32 boneId;
};

//start
struct fmtRE4_PMD {
	u32 magicTextLenght;
	char text[0x36];

	u32 SkeletonBoneNames_Length;
	SkeletonBoneName boneNames[SkeletonBoneNames_Length];

	u32 GroupMeshNames_Length;
	GroupMeshName groupMeshNames[GroupMeshNames_Length];

	padding[4];

	u32 skeleton_length;
	SkeletonAndParent skeletonAndParents[skeleton_length];
	
	u32 objNodes_Length;
	PMDnode nodes[objNodes_Length];

	u32 numberOfTextures;
	Material material[numberOfTextures];
};
	
fmtRE4_PMD re4pmd @ 0;