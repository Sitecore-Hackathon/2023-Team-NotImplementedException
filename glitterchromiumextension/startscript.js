// Get item information from Sitecore
function getCurrentItemInformation() {
    
    var itemId, itemLanguage, itemVersion, editor;
    
    var determineSitecoreEditor = function() {
        if(document.URL.indexOf("sitecore/shell/Applications/Content%20Editor.aspx") > -1) {
            editor = "contenteditor";
        } else {
            editor = "experienceeditor";
        }
        return editor;
    }


    var editor = determineSitecoreEditor();

    switch(editor) {
        case "experienceeditor":
            
            var itemIdElement = document.getElementById("scItemID");
            var itemLanguageElement = document.getElementById("scLanguage");
            var itemVersionElement = document.getElementById("scVersion");
            
            if (itemIdElement != null && itemLanguageElement != null) {
                itemId = itemIdElement.value;
                itemLanguage = itemLanguageElement.value; 
            }

            if (itemVersionElement != null) {
                itemVersion = itemVersionElement.value;
            }

            // if nothing was found, try to get the data from the querystring
            let params = new URLSearchParams(window.location.search);
            
            if (itemId == null) { 
                itemId = params.get("sc_itemid");
            }

            if (itemLanguage == null) { 
                itemLanguage = params.get("sc_lang");
            }

            if (itemVersion == null) { 
                itemVersion = params.get("sc_version");
            }


            // if itemVersion was not found found, try to get the itemversion from the ribbon :S
            if (itemVersion == null) {
                var element = document.getElementById('scWebEditRibbon').contentWindow.document.querySelectorAll('[data-sc-id="PageEditBar"]');
                
                if (element && element.length > 0) {
                    itemVersion = element[0].getAttribute("data-sc-version");
                }
            }
            
                       
            // fix guid format on itemid
            if (itemId != null && itemId.indexOf("-") == -1) {
                itemId = itemId.substring(0, 8) + "-" + itemId.substring(8, 12) + "-" + itemId.substring(12, 16) + "-" + itemId.substring(16, 20) + "-" + itemId.substring(20, 32);
            }
            
            if (itemId != null) {
                itemId = itemId.toLowerCase();
            }
            

            return {itemId, itemLanguage, itemVersion};
            
        case "contenteditor":
            // Experience editor
            var itemInfo = document.getElementById("__CurrentItem").value
            itemId = itemInfo.substring(itemInfo.indexOf("{"), itemInfo.indexOf("}") + 1);
            itemLanguage = itemInfo.substring(itemInfo.indexOf("lang=") + 5, itemInfo.indexOf("&ver="));
            itemVersion = itemInfo.substring(itemInfo.indexOf("ver=") + 4);
            if (itemId != null) {
                itemId = itemId.toLowerCase();
            }
            
            return {itemId, itemLanguage, itemVersion}
    }

    
}

// extension startup script
chrome.tabs.query({
    active: true,
    lastFocusedWindow: true
}, function(tabs) {
    var tab = tabs[0];
    chrome.scripting.executeScript({
        target : {tabId : tab.id},
        func : getCurrentItemInformation
      })
      .then(injectionResults => {
        for (const frameResult of injectionResults) {
          const {frameId, result} = frameResult;
            if(result && result.itemId != null && result.itemLanguage != null && result.itemVersion != null) {
                updateExtensionInterface(result.itemId, result.itemLanguage, result.itemVersion);
            } else {
                var historyElement = document.getElementById("history");
                historyElement.innerHTML = "No item information found<br/><br/>Please open an item in the content editor, experience editor or pages editor.";
            }
        }
      });
});


// update the extension gui with item information + glitter data
async function updateExtensionInterface(itemId, itemLanguage, itemVersion) {
    var historyElement = document.getElementById("history");
    let cleaneditemId = itemId.replace("{", "").replace("}", "");
    var apiData = await fetchApiData(cleaneditemId, itemLanguage, itemVersion);

    var historyItemTableElement = document.createElement("table");
    
    // render item data
    renderItemData(historyItemTableElement, "Item id", cleaneditemId);
    renderItemData(historyItemTableElement, "Item version", itemVersion);
    renderItemData(historyItemTableElement, "Item language", itemLanguage);
    renderDetailsLink(historyItemTableElement, cleaneditemId);

    historyElement.appendChild(historyItemTableElement);

    apiData.forEach(historyEntry => {
        var historyEntryElement = document.createElement("div");
        historyEntryElement.innerHTML = historyEntry.username + " - " + historyEntry.timestamp + " - " + historyEntry.fieldsText;
        historyElement.appendChild(historyEntryElement);
    });
}


// function to fetch data from the glitter API
async function fetchApiData(itemId, itemLanguage, itemVersion) {

    let url = "https://glitterbucket.localhost/item/" + itemId + "/language/" + itemLanguage + "/version/" + itemVersion;
    let response = await fetch(url);
    let historydata = await response.json();
    return historydata;
}


function renderItemData(parentElement, itemDataName, itemData) {
    
    const historyItemTableRowElement = document.createElement("tr");
    const historyItemTableCellHeaderElement = document.createElement("td");
    historyItemTableCellHeaderElement.className = "header";
    const historyItemTableCellDataElement = document.createElement("td");
    historyItemTableCellDataElement.className = "data";
    
    historyItemTableCellHeaderElement.innerHTML = itemDataName + ": ";
    historyItemTableCellDataElement.innerHTML = itemData;

    historyItemTableRowElement.appendChild(historyItemTableCellHeaderElement);
    historyItemTableRowElement.appendChild(historyItemTableCellDataElement);

    parentElement.appendChild(historyItemTableRowElement);
}


function renderDetailsLink(parentElement, itemId) {
    const elm = document.createElement("a");
    elm.className = "data";
    elm.href =  "https://kibana.localhost/app/dashboards#/view/54bf36f0-bae8-11ed-84ae-dbf8c3238367?_a=(filters:!((query:(match_phrase:(itemId.keyword:'" + itemId + "')))))";
    elm.innerHTML = "See details";
    elm.target = '_blank';

    parentElement.appendChild(elm);

}