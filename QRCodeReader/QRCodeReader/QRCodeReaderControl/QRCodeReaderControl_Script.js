//NOTE: alert() statements are available for debugging purposes. You can uncomment the statements to show dialogs when each method is hit.
(function ($) {
    //TODO: if necessary, add additional statements to initialize each part of the namespace before your QRCodeReaderControl is called. 
    if (typeof QRCodeReader === "undefined" || QRCodeReader == null) QRCodeReader = {};

    QRCodeReader.QRCodeReaderControl = {

        //internal method used to get a handle on the control instance
        _getInstance: function (id) {
            //alert("_getInstance(" + id + ")");
            var control = jQuery('#' + id);
            if (control.length == 0) {
                throw 'QRCodeReaderControl \'' + id + '\' not found';
            } else {
                return control[0];
            }
        },

        getValue: function (objInfo) {
            //alert("getValue() for control " + objInfo.CurrentControlId);
            var instance = QRCodeReader.QRCodeReaderControl._getInstance(objInfo.CurrentControlId);

            return instance.value;
        },

        getDefaultValue: function (objInfo) {
            //alert("getDefaultValue() for control " + objInfo.CurrentControlId);
            getValue(objInfo);
        },

        setValue: function (objInfo) {
            //alert("setValue() for control " + objInfo.CurrentControlId);
            var instance = QRCodeReader.QRCodeReaderControl._getInstance(objInfo.CurrentControlId);
            var oldValue = instance.value;
            //only change the value if it has actually changed, and then raise the OnChange event
            if (oldValue != objInfo.Value) {
                instance.value = objInfo.Value;
                raiseEvent(objInfo.CurrentControlId, 'Control', 'OnChange');
            }
        },

        //retrieve a property for the control
        getProperty: function (objInfo) {
            //alert("getProperty(" + objInfo.property + ") for control " + objInfo.CurrentControlId);
            if (objInfo.property.toLowerCase() == "value") {
                return QRCodeReader.QRCodeReaderControl.getValue(objInfo);
            }
            else {
                return $('#' + objInfo.CurrentControlId).data(objInfo.property);
            }
        },

        //set a property for the control. note case statement to call helper methods
        setProperty: function (objInfo) {
            switch (objInfo.property.toLowerCase()) {
                case "style":
                    QRCodeReader.QRCodeReaderControl.setStyles(null, objInfo.Value, $('#' + objInfo.CurrentControlId));
                    break;
                case "value":
                    QRCodeReader.QRCodeReaderControl.setValue(objInfo);
                    break;
                case "isvisible":
                    QRCodeReader.QRCodeReaderControl.setIsVisible(objInfo);
                    break;
                case "isenabled":
                    QRCodeReader.QRCodeReaderControl.setIsEnabled(objInfo);
                    break;
                default:
                    $('#' + objInfo.CurrentControlId).data(objInfo.property).value = objInfo.Value;
            }
        },

        validate: function (objInfo) {
            //alert("validate for control " + objInfo.CurrentControlId);
        },

        //helper method to set visibility
        setIsVisible: function (objInfo) {
            //alert("set_isVisible: " + objInfo.Value);
            value = (objInfo.Value === true || objInfo.Value == 'true');
            this._isVisible = value;
            var displayValue = (value === false) ? "none" : "block";
            var instance = QRCodeReader.QRCodeReaderControl._getInstance(objInfo.CurrentControlId);
            instance.style.display = displayValue;
        },

        //helper method to set control "enabled" state
        setIsEnabled: function (objInfo) {
            //alert("set_isEnabled: " + objInfo.Value);
            value = (objInfo.Value === true || objInfo.Value == 'true');
            this._isEnabled = value;
            var instance = QRCodeReader.QRCodeReaderControl._getInstance(objInfo.CurrentControlId);
            instance.readOnly = !value;
        },

        setStyles: function (wrapper, styles, target) {
            var isRuntime = (wrapper == null);
            var options = {};
            var element = isRuntime ? jQuery(target) : wrapper.find('.QRCodeReader.QRCodeReaderControl');

            jQuery.extend(options, {
                "border": element,
                "background": element,
                "margin": element,
                "padding": element,
                "font": element,
                "horizontalAlign": element
            });

            StyleHelper.setStyles(options, styles);
        },
        readqrcode: function (a) {
            var ctrl = $('.SFC.QRCodeReader-QRCodeReaderControl-Control');
            var ctrlval 
            ctrl[0].value = a;
            ctrl[0].text = a;
            //$('.SFC.QRCodeReader-QRCodeReaderControl-Control')[0].text = a;
            //this.value = a;
            //this.text = a;
            raiseEvent(ctrl[0].id, 'Control', 'OnChange');
            //alert(a);
                        
        }
    };
})(jQuery);

$(document).ready(function () {

    //add a delegate event handler for user-driven clicks 
    //TODO: add events for other user-driven events. 
    //(Note that custom controls created with the SDK have .SFC as the class)
    //you could also use event binding, if preferred 

    $(document).delegate('.SFC.QRCodeReader-QRCodeReaderControl-Control', 'click.Control', function (e) {
        //alert("control " + this.id + " clicked");
        raiseEvent(this.id, 'Control', 'OnClick');
    });

    $(document).delegate('.SFC.QRCodeReader-QRCodeReaderControl-Control', 'change.Control', function (e) {
        //alert("control " + this.id + " changed");
        raiseEvent(this.id, 'Control', 'OnChange');
    });
});
function handleFiles(f) {
    var o = [];

    for (var i = 0; i < f.length; i++) {
        var reader = new FileReader();
        reader.onload = (function (theFile) { return function (e) { qrcode.decode(e.target.result); }; })(f[i]);
        reader.readAsDataURL(f[i]);
    }
}
function load() {
    qrcode.callback = QRCodeReader.QRCodeReaderControl.readqrcode;
}

$(document).ready(function () {
    load();
});
