# XExten
--------------
#### 对LINQ进行了扩展和修改，扩展了表达式和httpclient的封装。支持Redis、mongodb、memorycache等缓存机制。通过emit创建动态类，读取XML，创建二维码，支持protobuf序列化，支持MessagePack序列化，支持简单的消息队列。
[![](https://img.shields.io/badge/build-success-brightgreen.svg)](https://github.com/EmilyEdna/XExten)
[![](https://img.shields.io/badge/nuget-v2.2.1.2-blue.svg)](https://www.nuget.org/packages/XExten/2.2.1.2)
![](https://img.shields.io/badge/support-Net461-blue.svg)
![](https://img.shields.io/badge/support-NetStandard2.1-blue.svg)
### 如何使用
--------------
##### 使用linq的拓展需要引入XExten.XCore域名空间
``` c#
[Fact]
public void ToOver_Test()
{
  List<TestA> Li = new List<TestA>();
  Li.ToOver(t => t.Name);
}
```
--------------
##### 使用expression的拓展需要引入XExten.XExpres域名空间
```c#
[Fact]
public void GetExpression_Test()
{
   string[] arr = new[] { "Id", "Name" };
   var res = XExp.GetExpression<TestA>(arr);
}
```
--------------
##### 使用Commom工具类需要引入XExten.XPlus域名空间
```c#
[Fact]
public void XBarHtml_Test()
{
    var res = XPlusEx.XBarHtml("ABC", 3, 50);
}
```
--------------
##### 使用HttpClient需要XExten.HttpFactory,使用DynamicType需要XExten.DynamicType.
##### 在项目中同时支持使用memoryCache,redis,mongodb
##### 具体如何使用消息队列测试
