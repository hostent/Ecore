/**
 * JS文件中使用 Locale.getText("key")读取国际化资源
 * @type {{getText: Function}}
 */
var MSG = MSG || {};
var Locale = {
    getText:function(key){
        if(key==undefined){
            return "";
        }
        var text = MSG[key];
        if(text!=undefined){
            return text;
        }else{
            return key;
        }
    }
}
var _T = Locale.getText; //使用_T(key)代替 Locale.getText(key)方法