﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CefSharp.Example.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CefSharp.Example.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE HTML PUBLIC &quot;-//W3C//DTD HTML 4.0 Transitional//EN&quot;&gt;
        ///&lt;html&gt;
        ///	&lt;head&gt;
        ///		&lt;title&gt;Binding Test&lt;/title&gt;
        ///	&lt;/head&gt;
        ///	&lt;body&gt;
        ///	    &lt;p&gt;
        ///	    Result of calling bound.Repeat(&quot;hi &quot;, 5) = 
        ///	    &lt;script type=&quot;text/javascript&quot;&gt;
        ///	        var result = bound.Repeat(&quot;hi &quot;, 5);
        ///	        document.write(&apos;&quot;&apos; + result + &apos;&quot;&apos;);
        ///	        if(result == &quot;hi hi hi hi hi &quot;) 
        ///	        {
        ///	            document.write(&quot; SUCCESS&quot;);
        ///	        }
        ///	        else
        ///	        {
        ///	            document.write(&quot; FAIL!&quot;);
        ///	        }	    [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BindingTest {
            get {
                return ResourceManager.GetString("BindingTest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap chromium256 {
            get {
                object obj = ResourceManager.GetObject("chromium256", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;body&gt;
        ///&lt;p id=&quot;instructions&quot;&gt;Select some portion of the below page content and click the &quot;Describe Selection&quot; button. The selected region will then be described below.&lt;/p&gt;
        ///&lt;p id=&quot;p1&quot;&gt;This is p1&lt;/p&gt;
        ///&lt;p id=&quot;p2&quot;&gt;This is p2&lt;/p&gt;
        ///&lt;p id=&quot;p3&quot;&gt;This is p3&lt;/p&gt;
        ///&lt;p id=&quot;p4&quot;&gt;This is p4&lt;/p&gt;
        ///&lt;form&gt;
        ///&lt;input type=&quot;button&quot; id=&quot;button&quot; value=&quot;Describe Selection&quot;&gt;
        ///&lt;p id=&quot;description&quot;&gt;The description will appear here.&lt;/p&gt;
        ///&lt;/form&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;
        ///.
        /// </summary>
        internal static string domaccess {
            get {
                return ResourceManager.GetString("domaccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE HTML&gt;
        ///&lt;html&gt;
        ///    &lt;head&gt;
        ///        &lt;title&gt;JavaScript Extension: Performance&lt;/title&gt;
        ///        &lt;style&gt;
        ///            body { font-family: Tahoma, Serif; font-size: 9pt; }
        ///        &lt;/style&gt;
        ///    &lt;/head&gt;
        ///    &lt;body&gt;
        ///        &lt;h1&gt;JavaScript Extension: Performance&lt;/h1&gt;
        ///
        ///        &lt;div&gt;&lt;span id=&quot;statusBox&quot;&gt;&lt;/span&gt; &lt;progress id=&quot;progressBox&quot; value=&quot;0&quot; style=&quot;display:none&quot;&gt;&lt;/progress&gt;&lt;/div&gt;
        ///
        ///        &lt;div style=&quot;padding-top:10px; padding-bottom:10px&quot;&gt;
        ///        &lt;table id=&quot;resultTable&quot; border=&quot;1&quot; cellspacing= [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string extensionperf {
            get {
                return ResourceManager.GetString("extensionperf", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;body&gt;
        ///&lt;script language=&quot;JavaScript&quot;&gt;
        ///var val = window.localStorage.getItem(&apos;val&apos;);
        ///function addLine() {
        ///  if(val == null)
        ///    val = &apos;&lt;br/&gt;One Line.&apos;;
        ///  else
        ///    val += &apos;&lt;br/&gt;Another Line.&apos;;
        ///  window.localStorage.setItem(&apos;val&apos;, val);
        ///  document.getElementById(&apos;out&apos;).innerHTML = val;
        ///}
        ///&lt;/script&gt;
        ///Click the &quot;Add Line&quot; button to add a line or the &quot;Clear&quot; button to clear.&lt;br/&gt;
        ///This data will persist across sessions if a cache path was specified.&lt;br/&gt;
        ///&lt;input type=&quot;button&quot; value=&quot;Add Line&quot; onClick=&quot;addLine( [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string localstorage {
            get {
                return ResourceManager.GetString("localstorage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!doctype html&gt;
        ///&lt;html&gt;
        ///&lt;head&gt;
        ///    &lt;title&gt;A Modal Dialog&lt;/title&gt;
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///Argument:&lt;input id=&quot;argument&quot; type=&quot;text&quot; size=&quot;32&quot;&gt;&lt;br&gt;
        ///&lt;br&gt;
        ///Reply:&lt;input id=&quot;reply&quot; type=&quot;text&quot; autofocus=&quot;autofocus&quot; size=&quot;32&quot;&gt;&lt;br&gt;
        ///&lt;p&gt;
        ///&lt;button onclick=&quot;OnOK(false)&quot;&gt;Cancel&lt;/button&gt; &lt;button onclick=&quot;OnOK(true)&quot;&gt;OK&lt;/button&gt;
        ///&lt;/p&gt;
        ///&lt;p id=&quot;time&quot;&gt;&lt;/p&gt;
        ///&lt;script&gt;
        ///              
        ///function init()
        ///{
        ///  timer();
        ///  setInterval(timer, 200);
        ///  setValueToId(&apos;argument&apos;, dialogArguments.msg);
        ///}
        ///
        ///function timer()
        ///{
        ///  updateI [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string modaldialog {
            get {
                return ResourceManager.GetString("modaldialog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!doctype html&gt; 
        ///&lt;html&gt; 
        ///&lt;head&gt; 
        ///&lt;title&gt;Test Modal Dialog&lt;/title&gt; 
        ///&lt;/head&gt; 
        ///&lt;body&gt;
        ///
        ///&lt;h3&gt;Tests&lt;/h3&gt;
        ///&lt;button onclick=&quot;doModal()&quot;&gt;Open the modal dialog&lt;/button&gt;&lt;br&gt;
        ///&lt;button onclick=&quot;window.close()&quot;&gt;Close this window&lt;/button&gt;
        ///
        ///&lt;h3&gt;Time (timers are suppresed while the modal dialog is open)&lt;/h3&gt;
        ///&lt;div id=&quot;time&quot;&gt;&lt;/div&gt;
        ///
        ///&lt;h3&gt;Result Log&lt;/h3&gt;
        ///&lt;div id=&quot;result&quot;&gt;&lt;/div&gt;
        ///
        ///&lt;script&gt;
        ///
        ///function init()
        ///{
        ///  timer();
        ///  setInterval(timer, 200);
        ///}
        ///
        ///function timer()
        ///{
        ///  updateId(&quot;time&quot;,new Date().toLocaleS [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string modalmain {
            get {
                return ResourceManager.GetString("modalmain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap nav_left_green {
            get {
                object obj = ResourceManager.GetObject("nav_left_green", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap nav_plain_green {
            get {
                object obj = ResourceManager.GetObject("nav_plain_green", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap nav_plain_red {
            get {
                object obj = ResourceManager.GetObject("nav_plain_red", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap nav_right_green {
            get {
                object obj = ResourceManager.GetObject("nav_right_green", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///&lt;title&gt;Off-Screen Rendering App Example&lt;/title&gt;
        ///&lt;/head&gt;
        ///&lt;body bottommargin=&quot;2&quot; rightmargin=&quot;0&quot; leftmargin=&quot;0&quot; topmargin=&quot;0&quot; marginwidth=&quot;0&quot; marginheight=&quot;0&quot; style=&quot;font-family: Verdana, Arial;&quot;&gt;
        ///&lt;div align=&quot;center&quot;&gt;
        ///&lt;table border=&quot;0&quot; cellpadding=&quot;0&quot; cellspacing=&quot;0&quot; width=&quot;99.9%&quot; height=&quot;100%&quot;&gt;
        ///&lt;tr&gt;
        ///   &lt;td height=&quot;100%&quot; align=&quot;center&quot; valign=&quot;top&quot;&gt;
        ///      &lt;table border=&quot;0&quot; cellpadding=&quot;2&quot; cellspacing=&quot;0&quot;&gt;
        ///      &lt;tr&gt;
        ///         &lt;td colspan=&quot;2&quot; style=&quot;font-size: 18pt;&quot;&gt;Off-Screen Rendering App Examp [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string osrplugin {
            get {
                return ResourceManager.GetString("osrplugin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE HTML PUBLIC &quot;-//W3C//DTD HTML 4.0 Transitional//EN&quot;&gt;
        ///&lt;html&gt;
        ///	&lt;head&gt;
        ///		&lt;title&gt;Scheme Handler Test&lt;/title&gt;
        ///	&lt;/head&gt;
        ///	&lt;body&gt;
        ///	&lt;h1&gt;Success&lt;/h1&gt;
        ///	&lt;p&gt;Scheme Handler Functioned Correctly&lt;/p&gt;
        ///	&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string SchemeTest {
            get {
                return ResourceManager.GetString("SchemeTest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE HTML PUBLIC &quot;-//W3C//DTD HTML 4.01 Transitional//EN&quot; &quot;http://www.w3.org/TR/html4/loose.dtd&quot;&gt;
        ///&lt;html&gt;
        ///&lt;head&gt;
        ///&lt;title&gt;Transparency Examples&lt;/title&gt;
        ///&lt;style type=&quot;text/css&quot;&gt;
        ///body {
        ///font-family: Verdana, Arial;
        ///}
        ///img {
        ///opacity:0.4;
        ///}
        ///img:hover {
        ///opacity:1.0;
        ///}
        ///#transbox {
        ///width: 300px;
        ///margin: 0 50px;
        ///background-color: #fff;
        ///border: 2px solid black;
        ///opacity: 0.3;
        ///font-size: 18px;
        ///font-weight: bold;
        ///}
        ///&lt;/style&gt;
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///
        ///&lt;h1&gt;Image Transparency&lt;/h1&gt;
        ///Hover over an image to make it fully opaque.&lt;br&gt;
        ///&lt;im [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string transparency {
            get {
                return ResourceManager.GetString("transparency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///&lt;title&gt;User Interface App Example&lt;/title&gt;
        ///&lt;/head&gt;
        ///&lt;body bottommargin=&quot;2&quot; rightmargin=&quot;0&quot; leftmargin=&quot;0&quot; topmargin=&quot;0&quot; marginwidth=&quot;0&quot; marginheight=&quot;0&quot; style=&quot;font-family: Verdana, Arial;&quot;&gt;
        ///&lt;div align=&quot;center&quot;&gt;
        ///&lt;table border=&quot;0&quot; cellpadding=&quot;0&quot; cellspacing=&quot;0&quot; width=&quot;99.9%&quot; height=&quot;100%&quot;&gt;
        ///&lt;tr&gt;
        ///   &lt;td height=&quot;100%&quot; align=&quot;center&quot; valign=&quot;top&quot;&gt;
        ///      &lt;table border=&quot;0&quot; cellpadding=&quot;2&quot; cellspacing=&quot;0&quot;&gt;
        ///      &lt;tr&gt;
        ///         &lt;td colspan=&quot;2&quot; style=&quot;font-size: 18pt;&quot;&gt;User Interface App Example&lt;/td&gt;
        ///     [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string uiplugin {
            get {
                return ResourceManager.GetString("uiplugin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;body&gt;
        ///&lt;script language=&quot;JavaScript&quot;&gt;
        ///function execXMLHttpRequest()
        ///{
        ///  xhr = new XMLHttpRequest();
        ///  xhr.open(&quot;GET&quot;,document.getElementById(&quot;url&quot;).value,false);
        ///  xhr.setRequestHeader(&apos;My-Custom-Header&apos;, &apos;Some Value&apos;);
        ///  xhr.send();
        ///  document.getElementById(&apos;ta&apos;).value = &quot;Status Code: &quot;+xhr.status+&quot;\n\n&quot;+xhr.responseText;
        ///}
        ///&lt;/script&gt;
        ///&lt;form&gt;
        ///URL: &lt;input type=&quot;text&quot; id=&quot;url&quot; value=&quot;http://tests/request&quot;&gt;
        ///&lt;br/&gt;&lt;input type=&quot;button&quot; onclick=&quot;execXMLHttpRequest();&quot; value=&quot;Execute XMLHttpRequest&quot;&gt;
        ///&lt;br/&gt;&lt;t [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string xmlhttprequest {
            get {
                return ResourceManager.GetString("xmlhttprequest", resourceCulture);
            }
        }
    }
}
