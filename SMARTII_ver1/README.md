# **SMART II ���N�t��** 

##### ��x�}�o���� (API SITE)
- .Net Framwork 4.6.1 (C# 7.0)
- Visual Studio 2015 �B 2017
- SSMS 2016
- ResX Manager (Extension)

##### �e�x�}�o���� (WEB SITE)
- Angular7 + Nebular/Bootstrap4 (ngx-Admin)
- ngrx (ver 7.+)
- node 10.16.0
- npm 6.9.0 
- Angular CLI 8.1.1
- ##### �Ұʪ`�N����
  - npm i -g node-gyp
  - node_module/@nebular/theme/styles/core/_breaking-notice.scss �� , @warn �������X�kchar �ݲ���
  - �Ѧ� https://github.com/akveo/ngx-admin/tree/master/src/app/pages


##### �t�ΰѦҤ��
- https://diagrams.visual-paradigm.com (�~�Ȭy�{)
- https://docs.google.com/spreadsheets/d/19qo8o3CWvIi3_mvbI2aAVPL2CbzfDo-Xq2HjrW6TUlQ/edit#gid=996562146 (�t�βӶ�)

#### �M�׵��c

```
SMARTII
�x 
�|�w�w�w doc
�x   �x 
�x   �|�w�w�w tools   
�x       �x 
�x       �x   docfx.json ( �۰ʲ��ͨt�Τ�� )
�x       �x   use.txt    ( �ϥΤⶶ )
�x       �|�w�w�w diagram   
�x       �x 
�x       �x   Domain_V1.classdiagram (vs2015)
�x       �x   Service_V1.classdiagram (vs2015)
�x          
�|�w�w�w build  
�x   �x   NuGet.Config ( Nuget�U�����|�]�w )
�x   
�|�w�w�w src
�x   �x 
�x   �|�w�w�w ap (.Net Framwork API)
�x       �x
�x       �|�w�w�w BusinessUnit   
�x           �x
�x           �|�w�w�w SMARTII.ASO (�̷�BU���S��欰���)
�x           �x    ...
�x           �x
�x           �x
�x       �|�w�w�w SMARTII  (�_�l�M��)
�x           �x
�x           �x
�x           �x
�x       �|�w�w�w SMARTII.Assist (���U�� , �B�z����)
�x           �x
�x           �x  
�x           �x  
�x       �|�w�w�w SMARTII.Domain ( ��������M�� )
�x           �x
�x           �x   
�x           �x   
�x       �|�w�w�w SMARTII.Resource (�h��y�t , ��L�귽..��)
�x           �x
�x           �x   
�x           �x   
�x       �|�w�w�w SMARTII.Service ( ��@�A���޿� )
�x           �x
�x           �x   
�x           �x 
�x       �|�w�w�w SMARTII.Tests 
�x           �x
�x           �x   
�x           �x
�x   �x 
�x   �|�w�w�w web (angular)
```

#### ���D
| ���x | ���D���n | �Բӱԭz | �����覡 | ������ |
| ------ | ------ |------ | ------  |------| 
| ��x | AD�n�J���� | ���Өt�εn�J�� , �ݰϤ��ӤH���O�_�� AD�b�� , �g��AD���ҫ� ,�t�α��v�n�J�C | �|������ |  �L |
| ��x | �_�����c�]�p | ��ץ�ݭn�̷Ӹӱ_�����s�i��d�߮� , �ݦҶq�q�����h�d�� , �B�U�{�į���D �C | �|������ |  �L |
| ��x | DLL ���c��� | ���Өt�Υi��|�]�� BU ���P , ���󤺮e�B�A�ȹ�@���P , �]���ݭn�ǥѤ���dll , �B��XAutomapper , Autofac , ����̷Ӥ��PBU�i��I�s�C | �Ƽg MutipleFormConverter �P JSONConverter �����ǤJ����, �ǥX���ثh�U�۹�@Autofac.Module �P Automapper.Profile  | ���O |
| ��x | �ץ�B�ӫ~�B��������Layout�w�q | ���Өt�Υi��|�]�� BU ���P , �i��|�ɭP����W�椣�P , ����ݥ��g�L�@��Ʃw�q , �S��ƫ᪺���Ҽ{�ǦC�Ƭ�JSON , �ë��[�ơC | �ݾ�X | ���O |
| ��x | �ʺA���� | �t�δ���������i�ۦ�w�q��X |  ���Ӧ��i��f�tPower BI �P WebApi �w�qReportLayout , �A�B�ϥΪ̤U��Desktop�ۦ�s��w�q�C  | �L |
| ��x | �q�������X | ���Өt�γq���欰��X , �ثe�� `SMS` `Email` `Webpush` `MobilePush` `LineMessage` `FacebookMessage`�ݹ�@ | Factory ����\�� , �è̾ڳq���覡�w�q���� | �L |
| �e�x�B��x | SignalR��X | ���Ӹܾ��i�u�B�ץ󲧰� ..�����ήɳq�� | �ݭn��e�x�إ�SignalRHub SingleInstance , ��x�]�ݷǳƦnHub , ������q | �L |
| �e�x�B��x | �ܰȾ�X | �ݰѷ�CallCenterCloud �M�� , �N�����X��s�M�פ� | �i�u���A�ݭn���եH�νT�{ |  �L |
| �e�x�B��x | �u��ʬݡBDashboard | �ݰѷ�CallCenterCloud �M�� , �N�����X��s�M�פ� |  �p�Բӱԭz | �L | 
| �e�x�B��x | �h�y�t | ��x�z�Lresx�޲z�h�y�t , �f�t�e�x @ngx-translate �i��y������ | �p�Բӱԭz | �L | 
| �e�x | Angular ��X | ��X ngx-Admin , �NPtc Component ��X�i�h (Select2 �BTreeView �BTable ...) | �p�Բӱԭz | ��ǡB���O |
| �e�x | Angular �������Χ@�~  |���Өt�Υi��]��BU���P , �Ӫ��󤺮e���P , �ݨ̷�BU����Module , �éw�q�S��ƫ᪺����BComponent...�� |  �p�Բӱԭz | ��ǡB���O |



#### �̿�M�� (PTC)
| �M�צW�� | ���| | �ԭz |
| ------ | ------ |------ |
| Ptc.Data.Condition2 | http://tfs2:8080/tfs/sd4-collection/LibrarySD4/_git/Condition2?path=%2FREADME.md&version=GBmaster&_a=preview | �]�˸�Ʀs���欰
| Ptc.Logger | http://tfs2:8080/tfs/sd4-collection/_git/LibrarySD4?path=%2FPtc.Logger | ������x�ɮ�


