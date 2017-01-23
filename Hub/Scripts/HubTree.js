
///////////////////////////////////////
///////////////////////////////////////
//
// INITIALIZE TREE
//
///////////////////////////////////////
///////////////////////////////////////
//

var HubTreeData = {};

function initializeTree(treeData, placeholderId, contentAreaId, contextActionsDataUrl) {

    ///////////////////////////////////////
    ///////////////////////////////////////
    //
    // NODE CSS CLASSES
    //
    ///////////////////////////////////////
    ///////////////////////////////////////

    HubTreeData.LinkClassExpanded = "hubTreeNodeImageLinkMinus";
    HubTreeData.LinkClassContracted = "hubTreeNodeImageLinkPlus";
    HubTreeData.LinkClassContentNode = "hubTreeElementContent";

    HubTreeData.HubNodeNameElement = "hubTreeElementNodeName";
    HubTreeData.HubNodeNoContent = "hubTreeElementNoContent";

    HubTreeData.HubNodeClass = "hubTreeElement";
    HubTreeData.HubNodeNoTreeOp = "hubTreeElementNoOpenClose"

    HubTreeData.ContentArea = contentAreaId;

    ///////////////////////////////////////
    ///////////////////////////////////////
    //
    // HTML RENDERING FUNCTIONS
    //
    ///////////////////////////////////////
    ///////////////////////////////////////
    function renderNodeExpandLink() {
        return "<a class='" + HubTreeData.LinkClassContracted + "' href=>&nbsp;</a>";
    }

    function renderContentLink(name, url) {
        return "<a class='" + HubTreeData.HubNodeNameElement + ' ' + HubTreeData.LinkClassContentNode + "' hub-content-url='" + url + "' href=>" + name + "</a>";
    }

    function renderNode(node) {

        nodeHtml = "";
        if (node.HasChildren) {
            nodeHtml += "<li id='" + node.Id + "' class='" + HubTreeData.HubNodeClass + "'>";
            nodeHtml += renderNodeExpandLink();
        }
        else {
            nodeHtml += "<li id='" + node.Id + "' class='" + HubTreeData.HubNodeClass + " " + HubTreeData.HubNodeNoTreeOp + "'>";
        }

        if (node.Content) {
            nodeHtml += renderContentLink(node.Name, node.Content.Url);
        }
        else {
            nodeHtml += "<span class='" + HubTreeData.HubNodeNameElement + " " + HubTreeData.HubNodeNoContent + "'>" + node.Name + "</span>";
        }

        nodeHtml += '</li>';
        return nodeHtml;
    }

    function renderContexMenuItem(linkItem) {

        link = "<li class='context-menu__item'>";
        link += "<a id='" + linkItem.id + "' href='' class='context-menu__link'>" + linkItem.name;
        link += "</a></li>";

        return link;
    }

    function renderContextMenu(linkCollection) {
        //var cMenu = "<nav id='context-menu' class='context-menu'>"
        var cMenu = "<ul id='context-menu' class='context-menu' style='list-style-type:none; text-align:left'>";

        jQuery.each(linkCollection, function (index, value) {
            cMenu += renderContexMenuItem(value);
        });

        cMenu += "</ul>"
        return cMenu;
    }


    ///////////////////////////////////////
    ///////////////////////////////////////
    //
    // EVENT BINDING AND PROCESSING
    //
    ///////////////////////////////////////
    ///////////////////////////////////////
    function bindTreeEvent(collection, callbackFunction) {
        jQuery.each(collection, function (key, value) {
            callbackFunction($(value));
        });
    }

    function bindContractionEvent(expandedNode) {
        expandedNode.click(function (evt) {

            //
            //Remove the inner <ul> of the parent li node
            var liElement = $(this).parents("." + HubTreeData.HubNodeClass).first();

            var ulO = liElement.children('ul:first-of-type');
            ulO.remove();

            //
            //Change the link class accordingly
            $(this).removeClass(HubTreeData.LinkClassExpanded).addClass(HubTreeData.LinkClassContracted);

            //
            //Unbind the contract event
            $(this).off("click");

            //
            //Bind expansion event
            bindExpansionEvent($(this));

            evt.preventDefault();
        });
    }

    function bindExpansionEvent(contractedNodeLink) {
        contractedNodeLink.click(function (evt) {

            var liElement = $(this).parents("." + HubTreeData.HubNodeClass).first();

            //call server, get children of element and render them
            $.ajax({
                type: "GET",
                dataType: "json",
                url: HubTreeData.hubTreeNodeExpansionUrl,
                data: {
                    nodeId: $(liElement).attr('id')
                },
                success: function (data, textStatus) {

                    //
                    //generate html from the result and insert it into a parent <ul> </ul>
                    var obj = jQuery.parseJSON(data)
                    var childrenHtml = "";

                    jQuery.each(obj, function (index, value) {
                        childrenHtml += renderNode(value);
                    });

                    $(liElement).append("<ul>" + childrenHtml + "</ul>");

                    var expansionLink = $(liElement).children("." + HubTreeData.LinkClassContracted).first()
                    expansionLink.removeClass(HubTreeData.LinkClassContracted).addClass(HubTreeData.LinkClassExpanded);

                    //
                    //on expansion find all contracted children
                    var contractedElements = $(liElement).find("." + HubTreeData.LinkClassContracted);

                    //
                    //and bind expansion event to them
                    bindTreeEvent(contractedElements, bindExpansionEvent);

                    //
                    //find all content nodes and bind their content event

                    var underlyingList = $(liElement).children("ul:first-of-type");

                    var contentElements = underlyingList.find("." + HubTreeData.LinkClassContentNode);
                    bindTreeEvent(contentElements, bindContentNode);

                    //
                    //remove the contraction event
                    expansionLink.off("click");

                    bindContractionEvent(expansionLink);
                }
            });

            evt.preventDefault();
        });
    }

    function bindContentNode(contentNodeLink) {

        contentNodeLink.click(function (evt) {

            //
            //get the url from node
            //ajax call the url and place data in the content area
            var url = $(this).attr('hub-content-url');
            var node = $(this).closest('.' + HubTreeData.HubNodeClass);

            //call server, get children of element and render them
            $.ajax({
                type: "GET",
                url: url,
                data: {
                    nodeId: node.attr('id')
                },
                success: function (data, textStatus) {
                    $('#'+HubTreeData.ContentArea).html(data);
                }
            });

            evt.preventDefault();
        });
    }

    function bindAddSiblingNode(siblingUrl) {

        $('a#hubAddSiblingNode').click(function (evt) {
            var el = jQuery.data(document.body, "lastClickedNode");
            $.ajax({
                type: "POST",
                dataType: "json",
                url: siblingUrl,
                data: {
                    nodeId: $(el).attr('id')
                },
                success: function (data, textStatus) {
                    var obj = jQuery.parseJSON(data)
                    var nodeHtml = renderNode(obj)
                    $(nodeHtml).insertAfter(el);
                }
            });
            evt.preventDefault();
        });
    }

    function bindAddChildNode(childUrl) {

        $('a#hubAddChildNode').click(function (evt) {

            var el = jQuery.data(document.body, "lastClickedNode");
            $.ajax({
                type: "POST",
                dataType: "json",
                url: childUrl,
                data: {
                    nodeId: $(el).attr('id')
                },
                success: function (data, textStatus) {
                    var obj = jQuery.parseJSON(data)
                    var nodeHtml = renderNode(obj)

                    //
                    //add to a parent without children
                    if ($(el).hasClass(HubTreeData.HubNodeNoTreeOp)) {
                        //render an expansion link and add it to the element
                        var link = $(renderNodeExpandLink());
                        $(el).removeClass(HubTreeData.HubNodeNoTreeOp);
                        $(el).prepend(link);
                        bindExpansionEvent(link);
                    }
                        //
                        //else the node has children
                    else {
                        //
                        //is the node expanded?
                        var expandedUL = $(el).children("ul:first-of-type");
                        if (expandedUL.length != 0) {
                            expandedUL.append(nodeHtml);
                        }
                    }
                }
            });
            evt.preventDefault();
        });
    }

    function bindRenameNode(renameUrl) {

        $('a#hubRenameNode').click(function (evt) {

            //How to rename a node
            //1. we have node names on their own within a span of class hubTreeElementNoContent
            //2. we also have node name within a link of class hubTreeElementContentLink
            //3. after the rename click => directly render the input box and bind enter key event
            //4. attach click handler to the ok button.
            //5. on button click: change the name on the server and remove the input box also changing the node name

            var el = jQuery.data(document.body, "lastClickedNode");
            //1, 2
            var nameElement = $(el).find("." + HubTreeData.HubNodeNameElement).first();
            var nameElementOldHtml = nameElement.html()

            var oldHrefAttrib = null;
            if (!nameElement.hasClass(HubTreeData.HubNodeNoContent)) {
                oldHrefAttrib = nameElement.attr('href');
                nameElement.removeAttr('href');
            }

            //3
            var id = "renameNode" + $(el).attr('id');
            var inputBox = "<input id='" + id + "' type='text' value='" + nameElement.text() + "'/>";

            nameElement.html(inputBox);

            //4
            $('input#' + id).bind("enterKey", function (e) {
                //5
                var value = $(this).val();
                var nodeId = $(this).parents("." + HubTreeData.HubNodeClass).first().attr('id');

                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: renameUrl,
                    data: {
                        nodeId: $(el).attr('id'),
                        name: value
                    },
                    success: function (data, textStatus) {
                        nameElement.html(nameElementOldHtml);
                        nameElement.text(value);

                        if (oldHrefAttrib != null) {
                            nameElement.attr('href', oldHrefAttrib);
                        }
                    }
                });
            });

            $('#' + id).keyup(function (e) {
                if (e.keyCode == 13) {
                    $(this).trigger("enterKey");
                }
            });

            evt.preventDefault();
        });
    }

    function bindDeleteNode(deleteUrl) {

        $('a#hubDeleteNode').click(function (evt) {
            var el = jQuery.data(document.body, "lastClickedNode");
            $.ajax({
                type: "POST",
                dataType: "json",
                url: deleteUrl,
                data: {
                    nodeId: $(el).attr('id')
                },
                success: function (data, textStatus) {

                    //
                    //node has class HubTreeData.HubNodeNoTreeOp meaning no children => simply remove => if the parent doesn't have other children reclass the parent and remove the expansion option
                    if ($(el).hasClass(HubTreeData.HubNodeNoTreeOp)) {
                        var siblings = $(el).siblings('.' + HubTreeData.HubNodeClass);

                        if (siblings.length == 0) {
                            var parent = $(el).parents('.' + HubTreeData.HubNodeClass).first();
                            parent.addClass(HubTreeData.HubNodeNoTreeOp);
                            parent.children('.' + HubTreeData.LinkClassExpanded).first().remove();

                            //we also need to delete the holding ul element
                            var ulO = parent.children('ul:first-of-type');
                            ulO.remove();
                        }
                    }
                    else {
                        //
                        //node has other class is it expanded? 
                        var link = $(el).children('.' + HubTreeData.LinkClassExpanded)
                        // => yes => get the children from the dom and reattach them as they have been
                        if (link.hasClass(HubTreeData.LinkClassExpanded)) {
                            var subList = $(el).children("ul:first-of-type");
                            var listElements = subList.children("." + HubTreeData.HubNodeClass);
                            for (i = 0; i < listElements.length; i++) {
                                element = $(listElements[i]).detach();
                                parent = $(el).parent();
                                element.appendTo(parent);
                            }
                            //
                            //=> no  => get the children from the server and attach them to the parent of the deleted node (which must be expanded, other wise we could not have deleted this node)
                        } else {
                            //take from the server data
                            var obj = jQuery.parseJSON(data)
                            var childrenHtml = "";

                            jQuery.each(obj, function (index, value) {
                                childrenHtml += renderNode(value);
                            });

                            var children = $(childrenHtml);

                            parent = $(el).parent();
                            children.appendTo(parent);

                            //
                            //bind expansion event if applicable
                            var contractedElements = children.find("." + HubTreeData.LinkClassContracted);
                            bindTreeEvent(contractedElements, bindExpansionEvent);
                        }
                    }
                    $(el).fadeOut('fast', function () { $(this).remove(); });                       //TODO: maybe make the whole tree opertions use fade in and out
                }
            });
            evt.preventDefault();
        });

    }

    function bindToContentNode(toContentUrl) {
        //
        //call the server and get the content node url
        //add the link,remove the span, change the classes
        //done
        $('a#hubToContentNode').click(function (evt) {

            // this is what we want
            //"<a class='" + HubTreeData.HubNodeNameElement + ' ' + HubTreeData.LinkClassContentNode + "' hub-content-url='" + node.Content.Url + "' href=>" + node.Name + "</a>";

            //this is what we have
            //"<span class='" + HubTreeData.HubNodeNameElement + " " + HubTreeData.HubNodeNoContent + "'>" + node.Name + "</span>";

            var el = jQuery.data(document.body, "lastClickedNode");
            var nameElement = $(el).find("." + HubTreeData.HubNodeNameElement).first();

            //2. call server and transform the section into a content section
            $.ajax({
                type: "POST",
                url: toContentUrl,
                data: {
                    nodeId: $(el).attr('id')
                },
                success: function (data, textStatus) {

                    //
                    //We get a complete node here
                    var obj = jQuery.parseJSON(data)

                    $(nameElement).replaceWith(renderContentLink(nameElement.text(), obj.Content.Url));
                    nameElement = $(el).find("." + HubTreeData.HubNodeNameElement).first();
                    bindContentNode(nameElement);
                }
            });
            evt.preventDefault();
        });
    }
    ///////////////////////////////////////
    ///////////////////////////////////////
    //
    // CONTEXT MENU
    //
    ///////////////////////////////////////
    ///////////////////////////////////////


    function initContextMenu() {

        "use strict";
        ///////////////////////////////////////
        ///////////////////////////////////////
        //
        // H E L P E R    F U N C T I O N S
        //
        ///////////////////////////////////////
        ///////////////////////////////////////

        function clickInsideElement(e, className) {
            var el = e.srcElement || e.target;


            if ($(el).hasClass(className)) {
                return el;
            } else {
                while (el = el.parentNode) {
                    if ($(el).hasClass(className)) {
                        return el;
                    }
                }
            }
            return false;
        }


        function getContextMenu() {
            var menu = $("#context-menu");
            return menu;
        }

        ///////////////////////////////////////
        ///////////////////////////////////////
        //
        // C O R E    F U N C T I O N S
        //
        ///////////////////////////////////////
        ///////////////////////////////////////

        /**
         * Variables.
         */

        var menuState = 0;
        var menuPosition;
        var menuPositionX;
        var menuPositionY;

        //var lastClickedItem;

        /**
         * Initialise our application's code.
         */
        function init() {
            contextListener();
            clickListener();
            keyupListener();
        }

        /**
         * Listens for contextmenu events.
         */
        function contextListener() {

            document.addEventListener("contextmenu", function (e) {

                var element = clickInsideElement(e, HubTreeData.HubNodeNameElement);

                if (element) {

                    jQuery.data(document.body, "lastClickedNode", $(element).parents('.' + HubTreeData.HubNodeClass).first());

                    e.preventDefault();
                    toggleMenuOn();
                    positionMenu(e);
                }
                else {
                    toggleMenuOff();
                }
            });
        }

        /**
         * Listens for click events.
         */
        function clickListener() {
            document.addEventListener("click", function (e) {
                var button = e.which || e.button;
                if (button === 1) {
                    toggleMenuOff();
                }
            });
        }

        /**
         * Listens for keyup events.
         */
        function keyupListener() {
            window.onkeyup = function (e) {
                if (e.keyCode === 27) {
                    toggleMenuOff();
                }
            }
        }

        /**
         * Turns the custom context menu on.
         */
        function toggleMenuOn() {

            var menu = getContextMenu();

            if (menuState !== 1) {
                menuState = 1;
                $(menu).show();
            }
        }

        function toggleMenuOff() {

            var menu = getContextMenu();

            if (menuState !== 0) {
                menuState = 0;
                $(menu).hide();
            }
        }

        function positionMenu(e) {

            var menu = getContextMenu();

            menuPosition = $(e.target).position(); //

            menuPositionX = menuPosition.left + "px";
            menuPositionY = menuPosition.top + 20 + "px";

            $(menu).css({ left: menuPositionX, top: menuPositionY });
        }

        /**
         * Run the app.
         */
        init();
    }

    //iterate over the treeData and render all
    var tree = "<ul>";
    var obj = jQuery.parseJSON(treeData);

    $.each(obj, function (key,value) {
        tree += renderNode(value);
    });

    tree += "</ul>";

    fff = $('#' + placeholderId);
    $('#' + placeholderId).append(tree);

    bindTreeEvent($("." + HubTreeData.LinkClassContracted), bindExpansionEvent);
    bindTreeEvent($("." + HubTreeData.LinkClassContentNode), bindContentNode);


    //
    //initialize the tree context menu
    var links = [
        {
            id: "hubAddSiblingNode",
            name: 'Add Sibling'
        },

        {
            id: "hubAddChildNode",
            name: 'Add Child'
        },

        {
            id: "hubRenameNode",
            name: 'Rename node'
        },

        {
            id: "hubDeleteNode",
            name: 'Delete node'
        },

        {
            id: "hubToContentNode",
            name: 'To content node'
        }
    ]

    if (contextActionsDataUrl != null) {

        $('#' + placeholderId).append(renderContextMenu(links));
        initContextMenu();

        //ContextMenuUrls
        $.ajax({
            type: "GET",
            url: contextActionsDataUrl,
            success: function (data, textStatus) {

                var obj = jQuery.parseJSON(data);

                HubTreeData.hubTreeNodeExpansionUrl = obj.hubTreeNodeExpansionUrl;
                bindAddSiblingNode(obj.bindAddSiblingNode);
                bindAddChildNode(obj.bindAddChildNode);
                bindRenameNode(obj.bindRenameNode);
                bindDeleteNode(obj.bindDeleteNode);
                bindToContentNode(obj.bindToContentNode);
            }
        });
    }
}