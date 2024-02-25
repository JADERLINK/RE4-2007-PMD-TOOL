# RE4-2007-PMD-TOOL
Extract and repack RE4 pmd files

Translate from Portuguese Brazil

Esses programas são uma versão refeita da versão do "magnum29" do programa "Re4 PMD to SMD model exporter" que foi escrito em "perl".
<br> A maior diferença com a verão dele, é a possibilidade da compressão de vértices, possibilitando arquivos .pmd menores.

**Update B.1.0.1.2**

Update de melhorias, agora serão criados os arquivos .idxpmdmaterial e .idxpmdbone; 
<br>agora, ao fazer repack a ordem das meches é definida pelo nome do material/grupo;

 ## RE4_2007_PMD_EXTRACT.exe

 Decodifica e cria um .obj/.smd do pmd. São criados os arquivos .txt2 .obj .smd .mtl e .idxpmd

* Sendo os arquivos txt2 arquivos informativos que divide o pmd por partes para melhor visualização, que por padrão não são gerados;
* .MTL: arquivo que acompanha o .obj, serve para carregar as texturas nos modelos, as texturas são arquivos .TGA e devem ficar na mesma pasta que o arquivo pmd/obj;
* .IDXPMD: é o arquivo que contém o conteúdo que não está no .obj ou no .smd, mas é necessário para recompilar o .PMD; (veja sobre o conteúdo do mesmo mais abaixo);
* .OBJ e .SMD: contem o conteúdo do modelo 3d;
* .idxpmdbone: contém o conteúdo dos bones, é usado somente ao fazer repack com o arquivo .OBJ;
* .idxpmdmaterial: arquivo que contém o material do modelo, pode ser usado como alternativa ao .mtl, para esse arquivo ser usado tem que ser habilitado no .idxpmd; 

 **RE4_2007_PMD_EXTRACT.txt**

 Esse arquivo encontra junto com a pasta do programa, é um arquivo de configurações para o programa, veja a baixo quais são elas:
 <br>Nota: são opções de "true" ou "false";

 * _EnableDebugFiles_: essa opção define se vai ser gerado o arquivo de debug .txt2, que é usado para ter informações do arquivo .pmd (normalmente usado por programadores);
 * _ReplaceMaterialNameByTextureName_: por padrão o nome dos materiais é nomeado como "PMD_MATERIAL_000" onde 000 é um numero, porem com essa opção ativa, o nome do material vai ser o nome da textura usada, ex: myTexture.tga
 (veja mais abaixo as diferença sobre isso)
 * _UseColorsInObjFile_: com essa opção ativa, no arquivo .obj nos parâmetros dos vértices, vai ter as cores de cada vértice, porem normalmente os editores 3D não tem suporte para isso.
 * _EnableUseIdxPmdMaterial_: variável que define a variável UseIdxPmdMaterial dentro do arquivo .idxpmd;

## RE4_2007_PMD_REPACK.exe

Cria um novo arquivo .PMD a partir do conteúdo do .IDXPMD e .OBJ ou .SMD (e também pode usar o .MTL):

* O programa recebe como entrada um arquivo .obj ou .smd, e também deve ter um arquivo idxpmd na mesma pasta com o mesmo nome do arquivo fornecido. (e também um arquivo .mtl);
* A compressão das vértices tem suporte nos arquivos SMD e OBJ, para ativar a função o arquivo .idxpmd deve conter a tag "CompressVertices:True", caso não queira comprimir as vértices mude "True" para "False".


 ## arquivo .idxPmd

Segue abaixo a lista de comando presente no arquivo .idxpmd, o // são comentários

```
// define que é para comprimir os vértices, recomendado manter como "True";
CompressVertices:True

//define se é um arquivo de cenário, só é true se for um arquivo da pasta "xscr";
IsScenarioPmd:False

//define qual Id do bone vai ser atribuído aos vértices, isso para o arquivo .obj;
ObjFileUseBone:0

//define se vai carregar as cores dos vértices do arquivo .obj;
LoadColorsFromObjFile: False

// define se vai carregar e usar o arquivo .mtl, no qual é onde vai pegar o nome da textura, (é usado no arquivo .obj e .smd), caso for "false", o nome da textura vai ser o nome do material.
UseMtlFile: True

// define se será usado o arquivo .idxpmdmaterial, para atribuir os materiais/texturas;
UseIdxPmdMaterial: False

// Conjunto de dados sobre os grupos, define o nome dos grupos, e quais materiais fazem parte de cada grupo;

// define se vai ser usado nomes de grupos customizados;
//UseCustomGroups:False

// quantidade de grupos
GroupsCount:23

//id do grupo:nome do grupo?id do SkeletonIndex?conjunto de materiais que fazem parte do grupo
Group_0:pl000a_Jacket_pl000a_Jacket_01_obj?43?PMD_MATERIAL_000
```

 ## arquivo .idxpmdbone

Contém o conteúdo dos bones, é usado somente ao fazer repack com o arquivo .OBJ;
<br>(Colocar aqui informativo sobre o arquivo)

## arquivo .idxpmdmaterial

Arquivo que contém o material do modelo, pode ser usado como alternativa ao .mtl, para esse arquivo ser usado, tem que ser habilitado no .idxpmd o campo "UseIdxPmdMaterial";
<br>(Colocar aqui informativo sobre o arquivo)

## Sobre como é defindo o nome da textura usada
O nome da textura é obtido considerando a seguinte ordem:
* Caso "UseIdxPmdMaterial" for "true", ele vai pegar o nome da textura a partir do arquivo .idxpmdmaterial caso tenha o nome do material listado;
* Caso "UseMtlFile" for "true" o nome da textura vai vir do arquivo .mtl;
* Caso, se tudo acima for "false" ou inválido, o nome da textura vai ser o próprio nome do material;

## Ordem dos bones no arquivo .SMD
Para arrumar a ordem dos ids dos bones nos arquivos smd, depois de serem exportados do blender ou outro software de edição de modelos usar o programa: GC_GC_Skeleton_Changer.exe (procure o programa no fórum do re4)

## Carregando as texturas no arquivo .SMD
No blender para carregar o modelo .SMD com as texturas, em um novo "projeto", importe primeiro o arquivo .obj para ele carregar as texturas, delete o modelo do .obj importado, agora importe o modelo .smd, agora ele será carregado com as texturas.(lembrando que os arquivos .tga devem estar na mesma parta que o .OBJ/.SMD./PMD)

## Escala
A escala dos arquivo Pmd da pasta xfile, são armazenado de forma a ser 100x menor que a escala real do jogo.
<br> E dos arquivos Pmd da pasta xscr são 1000x menores que a escala real do jogo (porem também tem outras escalas internas).

## Código de terceiro:

[ObjLoader by chrisjansson](https://github.com/chrisjansson/ObjLoader):
Encontra-se no RE4_PMD_Repack, código modificado, as modificações podem ser vistas aqui: [link](https://github.com/JADERLINK/ObjLoader).

## RE4_PMD.hexpat
No source code, disponibilizei o arquivo RE4_PMD.hexpat, que é um arquivo para ser usado no programa "[ImHex](https://imhex.werwolv.net/)", serve para visualizar a estrutura do arquivo Pmd.

**At.te: JADERLINK**
<br>2024-02-25
