
var f_log = 1 ? console.log.bind(console, '[LOG] ') : function () { };

var _SELECT_OBJ = { x: 0, y: 0, text: '', id: '' };

function f_event_processCenter(event) {
    var type = event.type,
        el = event.target,
        tagName = el.tagName,
        id = el.id,
        text = el.innerText,
        textSelect = '';

    if (id == null || id.trim().length == 0) {
        var id = _GET_ID();
        el.setAttribute('id', id);
    }

    textSelect = window.getSelection().toString();
    switch (type) {
        case 'mousedown':
            if (console.clear) console.clear();

            var elbox = document.getElementById('___box_tran');
            if (elbox) elbox.style.display = 'none';

            _SELECT_OBJ = { id: id, cached: false, x: event.x, y: event.y };
            if (el.className == '___translated') _SELECT_OBJ.cached = true;

            break;
        case 'mouseup':
            if (_SELECT_OBJ != null) {
                _SELECT_OBJ.x = event.x;
                _SELECT_OBJ.y = event.y;
                if (textSelect && textSelect.trim().length > 0) _SELECT_OBJ.text = textSelect;
            }
            break;
        case 'click':
            if (_SELECT_OBJ != null) {
                if (textSelect && textSelect.trim().length > 0) _SELECT_OBJ.text = textSelect;
            }
            break;
        case 'dblclick':
            if (_SELECT_OBJ != null) {
                if (textSelect && textSelect.trim().length > 0) _SELECT_OBJ.text = textSelect;
            }
            break;
    }

    //f_log(tagName + '.' + type + ': ' + JSON.stringify(_SELECT_OBJ));

    if (_SELECT_OBJ != null) {
        if (_SELECT_OBJ.cached == true) {
            f_displayTranslateCache(_SELECT_OBJ);
            _SELECT_OBJ = null;
        } else {
            if (_SELECT_OBJ.text && _SELECT_OBJ.text.length > 0) {
                f_sendHTTPS(_MSG_TYPE.EN_TRANSLATE_GOOGLE_REQUEST, _SELECT_OBJ);
                _SELECT_OBJ = null;
            }
        }
    }

    //f_log(tagName + '.' + type + ': ' + id + ' \r\nSELECT= ' + textSelect + ' \r\nTEXT= ', text);
    //event.preventDefault();
    //event.stopPropagation();
}

if (window.addEventListener) {
    window.addEventListener("mouseup", f_event_processCenter, true);
    window.addEventListener("mousedown", f_event_processCenter, true);
    window.addEventListener("click", f_event_processCenter, true);
    window.addEventListener("dblclick", f_event_processCenter, true);
}

function f_go(url, title) {
    document.body.innerHTML = '<h1>' + title + '<br><br><br><br><br><img src="data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9Im5vIj8+PHN2ZyB4bWxuczpzdmc9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHhtbG5zOnhsaW5rPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5L3hsaW5rIiB2ZXJzaW9uPSIxLjAiIHdpZHRoPSI2NHB4IiBoZWlnaHQ9IjY0cHgiIHZpZXdCb3g9IjAgMCAxMjggMTI4IiB4bWw6c3BhY2U9InByZXNlcnZlIj48Zz48cGF0aCBkPSJNNzEgMzkuMlYuNGE2My42IDYzLjYgMCAwIDEgMzMuOTYgMTQuNTdMNzcuNjggNDIuMjRhMjUuNTMgMjUuNTMgMCAwIDAtNi43LTMuMDN6IiBmaWxsPSIjMDAwIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iI2UxZTFlMSIgdHJhbnNmb3JtPSJyb3RhdGUoNDUgNjQgNjQpIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iI2UxZTFlMSIgdHJhbnNmb3JtPSJyb3RhdGUoOTAgNjQgNjQpIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iI2UxZTFlMSIgdHJhbnNmb3JtPSJyb3RhdGUoMTM1IDY0IDY0KSIvPjxwYXRoIGQ9Ik03MSAzOS4yVi40YTYzLjYgNjMuNiAwIDAgMSAzMy45NiAxNC41N0w3Ny42OCA0Mi4yNGEyNS41MyAyNS41MyAwIDAgMC02LjctMy4wM3oiIGZpbGw9IiNiZWJlYmUiIHRyYW5zZm9ybT0icm90YXRlKDE4MCA2NCA2NCkiLz48cGF0aCBkPSJNNzEgMzkuMlYuNGE2My42IDYzLjYgMCAwIDEgMzMuOTYgMTQuNTdMNzcuNjggNDIuMjRhMjUuNTMgMjUuNTMgMCAwIDAtNi43LTMuMDN6IiBmaWxsPSIjOTc5Nzk3IiB0cmFuc2Zvcm09InJvdGF0ZSgyMjUgNjQgNjQpIi8+PHBhdGggZD0iTTcxIDM5LjJWLjRhNjMuNiA2My42IDAgMCAxIDMzLjk2IDE0LjU3TDc3LjY4IDQyLjI0YTI1LjUzIDI1LjUzIDAgMCAwLTYuNy0zLjAzeiIgZmlsbD0iIzZlNmU2ZSIgdHJhbnNmb3JtPSJyb3RhdGUoMjcwIDY0IDY0KSIvPjxwYXRoIGQ9Ik03MSAzOS4yVi40YTYzLjYgNjMuNiAwIDAgMSAzMy45NiAxNC41N0w3Ny42OCA0Mi4yNGEyNS41MyAyNS41MyAwIDAgMC02LjctMy4wM3oiIGZpbGw9IiMzYzNjM2MiIHRyYW5zZm9ybT0icm90YXRlKDMxNSA2NCA2NCkiLz48YW5pbWF0ZVRyYW5zZm9ybSBhdHRyaWJ1dGVOYW1lPSJ0cmFuc2Zvcm0iIHR5cGU9InJvdGF0ZSIgdmFsdWVzPSIwIDY0IDY0OzQ1IDY0IDY0OzkwIDY0IDY0OzEzNSA2NCA2NDsxODAgNjQgNjQ7MjI1IDY0IDY0OzI3MCA2NCA2NDszMTUgNjQgNjQiIGNhbGNNb2RlPSJkaXNjcmV0ZSIgZHVyPSI3MjBtcyIgcmVwZWF0Q291bnQ9ImluZGVmaW5pdGUiPjwvYW5pbWF0ZVRyYW5zZm9ybT48L2c+PGc+PGNpcmNsZSBmaWxsPSIjMDAwIiBjeD0iNjMuNjYiIGN5PSI2My4xNiIgcj0iMTIiLz48YW5pbWF0ZSBhdHRyaWJ1dGVOYW1lPSJvcGFjaXR5IiBkdXI9IjcyMG1zIiBiZWdpbj0iMHMiIHJlcGVhdENvdW50PSJpbmRlZmluaXRlIiBrZXlUaW1lcz0iMDswLjU7MSIgdmFsdWVzPSIxOzA7MSIvPjwvZz48L3N2Zz4="/></h1>';
    //setTimeout(function () { API.GoTitle(url, title); }, 3000);
    API.GoTitle(url, title);
}

function f_domLoaded() {
    var elTitle = document.getElementById('___title');
    if (elTitle) document.title = elTitle.value;

    //console.info("JS-> " + location.href);

    var URL = location.href,
        aHost = location.hostname.split('.'),
        DOMAIN_MAIN = aHost.length > 1 ? (aHost[aHost.length - 2] + '.' + aHost[aHost.length - 1]) : aHost[0];
    aHost = URL.split('/');
    var URL_SCHEME = aHost[0].substring(0, aHost[0].length - 1),
        HTTP_ROOT = URL_SCHEME + '://' + aHost[2] + '/',
        URL_DIR = URL;
    if (URL[URL.length - 1] != '/')
        URL_DIR = URL.substring(0, URL.length - URL[URL.length - 1].length);

    f_log("----> DOM loaded: URL_SCHEME = " + URL_SCHEME);
    f_log("----> DOM loaded: DOMAIN_MAIN = " + DOMAIN_MAIN);
    f_log("----> DOM loaded: URL_DIR = " + URL_DIR);
    f_log("----> DOM loaded: HTTP_ROOT = " + HTTP_ROOT);

    var links = document.querySelectorAll('a'),
        aLink = [];
    for (var i = 0; i < links.length; i++) {
        var el = links[i], text = el.innerText;
        if (text != null && text.length > 0 && el.hasAttribute('href') == true) {
            text = text.trim();
            if (text.length == 0 || text[0] == '#') { el.parentElement.removeChild(el); continue; }
            var link = el.getAttribute('href').trim();
            if (link.length == 0 || link[0] == '#') { el.parentElement.removeChild(el); continue; }

            if (link.indexOf(DOMAIN_MAIN) != -1 || link.indexOf('http') != 0) {
                text = text.split("'").join('').split('"').join('');

                var link_full = '';
                if (link.indexOf('//') == 0)
                    link_full = URL_SCHEME + ':' + link;
                else {
                    if (link.indexOf('http') != 0) {
                        if (link[0] == '/') link_full = HTTP_ROOT + link.substr(1);
                        else if (link.indexOf('../') == 0) { }
                        else {
                            link_full = URL_DIR + link;
                        }
                    }
                }

                if (link_full.length > 0) {
                    //f_log('A = ' + link + ' -> ' + link_full);
                    link = link_full;
                }

                if (URL == link || URL == link + '/' || link.indexOf('javascript:') != -1) continue;

                //el.parentElement.removeChild(el);
                el.setAttribute("href", "javascript:void(-1);");
                el.setAttribute("onclick", "f_go('" + link + "','" + text + "');");

                aLink.push({ Text: text, Url: link });
            } else { el.parentElement.removeChild(el); continue; }
        } else { el.parentElement.removeChild(el); continue; }
    }

    if (aLink.length > 0) {
        var json = JSON.stringify(aLink);
        setTimeout(function (_json) { API.UpdateLinks(_json); }, 100, json);
    }

    var imgs = document.querySelectorAll('.___img_src');
    for (var i = 0; i < imgs.length; i++) {
        var el = imgs[i], s = el.value;
        if (s != null && s.length > 0) {
            //f_log('IMG = ' + s);
        }
    }

    var els = document.querySelectorAll('*');
    for (var i = 0; i < els.length; i++) {
        var el = els[i], s = el.innerText;
        if (s == null || s == undefined || s.length == 0) {
            //f_log('type = ' + el.type + '; name = ' + el.name + '; text = ' + s);
            el.style.display = 'none';
        }
    }

}

function f_getTextWidth(text, font) {
    var c = document.getElementById("___canvas");
    var ctx = c.getContext("2d");
    ctx.font = font;
    return ctx.measureText(text).width;
}

function f_displayTranslateCache(oTran) {
    //alert(JSON.stringify(oTran));
    if (oTran && oTran.id) {
        var el = document.getElementById(oTran.id);
        if (el) {
            var text = el.innerText, s = '';
            if (el.hasAttribute('title')) s = el.getAttribute('title');
            oTran.success = true;
            oTran.text = text;
            oTran.mean_vi = s;
            f_displayTranslate(oTran);

            f_log('CACHE: BROWSER -> SETTING = ' + JSON.stringify(oTran));
            f_sendSETTING(_MSG_TYPE.EN_TRANSLATE_GOOGLE_RESPONSE, null, oTran);
        }
    }
}

function f_displayTranslate(oTran) {
    //alert(JSON.stringify(oTran));
    var el = document.getElementById('___box_tran');
    if (el && oTran) {
        var s = oTran.mean_vi;
        if (s == null || s.length == 0) {
            window.getSelection().empty();
            return;
        }

        s = s[0].toUpperCase() + s.substr(1);
        var wiBrowser = APP_INFO.Width - APP_INFO.AreaLeft.Width;
        var wiText = f_getTextWidth(s, '0.7em Arial') + 25;

        if (wiText > wiBrowser) {
            oTran.x = 40;
            el.style.width = (wiBrowser - 99) + 'px';
            el.style.top = (oTran.y + 25 + window.pageYOffset) + 'px';
        }
        else {
            el.style.width = wiText + 'px';
            el.style.top = (oTran.y + 21 + window.pageYOffset) + 'px';
            if (oTran.x + wiText > wiBrowser) oTran.x = wiBrowser - (wiText + 50);
        }

        el.innerHTML = s;
        el.style.left = oTran.x + 'px';
        el.style.display = 'inline-block';

        if (oTran.cached == null || oTran.cached == false) {
            var sel = window.getSelection();
            if (sel.rangeCount) {
                range = sel.getRangeAt(0);
                range.deleteContents();
                var node = document.createElement('i');
                node.className = '___translated';
                node.setAttribute('title', s);
                var id = oTran.id;
                node.setAttribute('id', id);
                node.innerHTML = oTran.text;
                //range.insertNode(document.createTextNode('[' + s + ']'));
                range.insertNode(node);
            }
            window.getSelection().empty();
        }
    }
}