# RE4-2007-PMD-TOOL
Extract and repack RE4 pmd files

Translate from Portuguese Brazil

Esses programas são uma versão refeita da versão do "magnum29" do programa "Re4 PMD to SMD model exporter" que foi escrito em "perl".
<br> A maior diferença com a verão dele, é a possibilidade da compressão de vértices, possibilitando arquivos .pmd menores.

**Update B.1.0.0.1**

Corrigido um bug no repack no qual a variavel "UseMaterialLines" do .idxpmd era sempre falsa, impossibilitando o uso do campo "MaterialLine".


 ## RE4_PMD_Decoder.exe

 Decodifica e cria um .obj/.smd do pmd. São criados os arquivos .txt2 .obj .smd .mtl e .idxpmd

 * sendo os arquivos txt2 arquivos informativos que divide o pmd por partes para melhor visualização, que por padrão não são gerados;
 * .MTL: arquivo que acompanha o .obj, serve para carregar as texturas nos modelos, as texturas são arquivos .TGA e devem ficar na mesma pasta que o arquivo pmd/obj;
 * .IDXPMD: é o arquivo que contem o conteúdo que não esta no .obj ou no .smd, mas é necessário para recompilar o .PMD; (veja sobre o conteúdo do mesmo mais abaixo)
 * .OBJ e .SMD: contem o conteúdo do modelo 3d.

 **RE4_PMD_Decoder.txt**

 Esse arquivo encontra junto com a pasta do programa, é um arquivo de configurações para o programa, veja a baixo quais são elas:
 <br>Nota: são opções de "true" ou "false";

 * _EnableDebugFiles_: essa opção define se vai ser gerado o arquivo de debug .txt2, que é usado para ter informações do arquivo .pmd (normalmente usado por programadores);
 * _ReplaceMaterialNameByTextureName_: por padrão o nome dos materiais é nomeado como "PMD_MATERIAL_000" onde 000 é um numero, porem com essa opção ativa, o nome do material vai ser o nome da textura usada, ex: myTexture.tga
 (veja mais abaixo as diferença sobre isso)
 * _UseColorsInObjFile_: com essa opção ativa, no arquivo .obj nos parâmetros dos vértices, vai ter as cores de cada vértice, porem normalmente os editores 3D não tem suporte para isso.

## RE4_PMD_Repack.exe

Cria um novo arquivo .PMD a partir do conteúdo do .IDXPMD e .OBJ ou .SMD (e também pode usar o .MTL):

* O programa recebe como entrada um arquivo .obj ou .smd, e também deve ter um arquivo idxpmd na mesma pasta com o mesmo nome do arquivo fornecido. (e também um arquivo .mtl);
* A compressão das vértices tem suporte nos arquivos SMD e OBJ, para ativar a função o arquivo .idxpmd deve conter a tag "CompressVertices:True", caso não queira comprimir as vértices mude "True" para "False".


 ## arquivo .idxPmd

Segue abaixo a lista de comando presente no arquivo .idxpmd, o // são comentários

// define que é para comprimir as vertices, recomendado manter como "True"
<br>CompressVertices:True
<br>//define se é um arquivo de cenário, so é true se for um arquivo da pasta "xscr"
<br>IsScenarioPmd:False
<br>
<br>// conjunto de dados sobre o bone, só é usado para o arquivo .obj
<br>
<br>// para o arquivo obj, define qual bone será usado pela malha/mesh
<br>ObjFileUseBone:0
<br>// quantidade de bones
<br>BonesCount:69
<br>//o "0" é o id do bone, e o campo a baixo é o nome do bone
<br>BoneLine_0_Nome:Scene_Root
<br>// parent do bone
<br>BoneLine_0_Parent:-1
<br>//conjunto de 26 campos float
<br>BoneLine_0_Data: //campos omitidos nesse exemplo
<br>/ fim do conjunto de bones
<br>
<br>// conjunto de dados sobre os grupos, define o nome dos grupos, e quais materiais faz parte de cada grupo
<br>
<br>// quantidade de grupos
<br>GroupsCount:23
<br>id do grupo:nome do grupo?id do SkeletonIndex?conjunto de materiais que fazem parte do grupo
Group_0:pl000a_Jacket_pl000a_Jacket_01_obj?43?PMD_MATERIAL_000
<br>
<br> //define se vai carregar as cores dos vértices do arquivo .obj
<br> LoadColorsFromObjFile: False
<br>
<br>// define se vai carregar e usar o arquivo .mtl, no qual é onde vai pegar o nome da textura,(é usado no arquivo .obj e .smd), caso for "false", o nome da textura vai ser o nome do material.
<br>UseMtlFile: True
<br>
<br>// define se sera usado a tabela abaixo de materiais (é definido como "false" por padrão)
<br>UseMaterialLines: False
<br>
<br>//lista de materiais
<br>//MaterialLine?nome do material?no da textura usada: conteúdo de 17 float e 1 int (são parâmetro da textura)
<br>MaterialLine?PMD_MATERIAL_000?pl0011.tga: //conteúdo omitido
<br> **Fim do arquivo .idxpmd**

**Sobre como é defindo o nome da textura usada**
<br>O nome da textura é obtida considerando a seguinte ordem:
* caso UseMaterialLines for "true", ele vai pegar o nome da textura a partir de MaterialLine caso tenho o nome do material listado.
* caso UseMtlFile for "true" o nome da textura vai vir do arquivo .mtl
* caso se tudo acima for "false" ou invalido, o nome da textura vai ser o próprio nome do material

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

**At.te: JADERLINK**
<br>2023-10-02
