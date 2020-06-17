#### Ϊ�ֲ�ʽϵͳ�ṩ�ļ򵥵�ϵͳ׷�����
[![](https://img.shields.io/badge/build-success-brightgreen.svg)](https://github.com/EmilyEdna/XExten.Profile)
[![](https://img.shields.io/badge/nuget-v1.0.0-blue.svg)](https://www.nuget.org/packages/XExten.Profile/1.0.0)
## ǰ��
=============
Ŀǰ����֧��SQLServer,EFCore,AspNetCore,HttpClient,Method�ĸ��٣����ǰ��4������ʹ����ֻ��Ҫ�ڳ���Startup��ע�뼴�ɣ���Method��Ȼ��
## ʹ�÷���
------------------------------------
1.��AspNetCore��Startup������ע��APM
``` c#
    services.RegistXExtenService();
```
2.����ʹ��UI����ͬ����Startupע��:
``` c#
     app.UseTraceUI();
```
3.HttpֻҪ����System.Net��ؾͻ��Զ�ִ������:
``` c#
    HttpMultiClient.HttpMulti.AddNode("https://www.baidu.com").Build().RunString();
```
4.SQL��ֻҪִ��SQL��ؾͻ��Զ�ִ������:
```c#
   (new DbContext()).warnInfos.FirstOrDefault();
```
5.����Ҫʵ��Method�ĸ���������ģʽ��һ��ͨ����ʽһʵ�֣��ڶ������Զ���proxyʵ��:
```c#
    //��ʽһ
    TestClass tc = new TestClass();
    var data = ResultProvider.SetValue("Name", new Dictionary<object, object> { { "Key", "Value" } });
    tc.ByTraceInvoke("TestMethods", new object[] { data });
    //��ʽ���Ǹ��ݿͻ��Լ��Զ���ķ�ʽʵ�����ﲻ����ʾ
```
#UI���ص�ַ
https://github.com/EmilyEdna/XExten/Release
