
//针对表格内的时间格式转换
function dateTab(dateClass) {
    dateClass.each(function (i, m) {
        m.innerText = (m.innerText.split("/周"))[0];
    })
}
//针对表格内中input的时间格式转换
function dateInput(dateClass) {
    dateClass.each(function (i, m) {

        m.value = (m.value.split("/周"))[0];
    })
}
//刷新页面对主表零售合计进行订正
function suerTotal(url,id) {
    EditMain(url, id);
}


//修改主表

function EditMain(url,datas) {
    $.ajax({
        url: url,
        type: 'post',
        data: datas,
        dataType: 'json',
        success: function (data) {
            console.log(data);
        },
        error: function (err) {
            console.log(err);
        }
    })
}
//删除主表
function DelAll(url, id, nextPage) {
    if (confirm("如果删除此订单，此订单的所有商品也会一并删除，是否继续？")) {
        $.ajax({
            url: url,
            type: 'post',
            data: { id: id },
            dataType: 'json',
            success: function (data) {
                console.log(data);
                if (data.success) {
                   
                    alert(data.msg + "跳转至销售列表。");
                    window.location.href = nextPage;
                   
                    
                } else {
                    alert(data.msg);
                }
            },
            error: function (err) {
                console.log(err);
            }
        })
    }
}
//删除商品
function DelGood(url, id) {
    if (confirm("确认删除该商品？")) {
        $.ajax({
            url: url,
            type: 'post',
            data: { id: id },
            dataType: 'json',
            success: function (data) {
                console.log(data);
                if (data.success) {
                    alert(data.msg);
                    window.location.reload();
                } else {
                    alert(data.msg);
                }
            },
            error: function (err) {
                console.log(err);
            }
        })
    }
}

//goods用于
var goods = null,//用于sku获取的商品信息赋值
    billsID = null,
    detailUrl = null;//用于单据ID
   
function getbillsID(url,id) {
    billsID = id;
    detailUrl = url;
}
$("#Addgoods").click(function () {
    getStore();
    $("#add-dialog").dialog("option", "title", "查找单品").dialog("open");
})
/***************修改销售订单详情时的商品添加************/
function getStore() {
    //获取商品分类
    $.ajax({
        url: 'http://api.chiccasa.com.cn/ChicERP.asmx/Get_Category',// api地址/asmx 是WEB服务文件/方法      api地址:47.98.38.206:8089/test.asmx,方法是abc
        type: 'post',
        data: {},//传入的参数
        contentType: "application/json;charset=utf-8",//contentType: 发送信息至服务器时内容编码类型，简单说告诉服务器请求类型的数据
        dataType: 'json',
        success: function (data) {
            var a = JSON.parse(data.d);
            console.log(a)
            $(a).each(function (i, m) {
                $("#classify").append("<option value='" + m.id + "'>" + m.Name + "</option>")
            })

        },
        error: function (err) {
            console.log(err);
        }
    })
}
//根据商品分类 获取商品型号
$("#classify").change(function () {
    //清空商品型号 产品列表 参数列表
    $("#model").empty();
    $("#productList").empty();
    $("#parameter tbody").empty();
    $("#SKU p").text("");

    let CaegoryID = $("#classify").val();
    $.ajax({
        url: 'http://api.chiccasa.com.cn/ChicERP.asmx/Get_Model',
        type: 'post',
        data: "{_CaegoryID:" + CaegoryID + "}",//传入的参数
        contentType: "application/json;charset=utf-8",
        dataType: 'json',
        success: function (data) {
            var a = JSON.parse(data.d);
            //console.log(a)
            $("#model").append("<option value=''  class='active'>-请选择商品型号-</option>")
            $(a).each(function (i, m) {
                $("#model").append("<option value='" + m.id + "'  >" + m.Model_Name + '-' + m.Series_Name + "</option>");
            })

        },
        error: function (err) {
            console.log(err);
        }
    })
})
//根据商品型号 获取产品列表
$("#model").change(function () {
    activeText($("#model"));
    var id = $("#model").val();
    $("#productList").empty();
    $("#parameter tbody").empty();
    $("#SKU p").text("");
    $.ajax({
        url: 'http://api.chiccasa.com.cn/ChicERP.asmx/Get_ProductList',
        type: 'post',
        data: "{_ModelID:" + id + "}",//传入的参数
        contentType: "application/json;charset=utf-8",
        dataType: 'json',
        success: function (data) {
            var a = JSON.parse(data.d);
            //console.log(a)
            $("#productList").append("<option value=''>-请选择产品-</option>")
            $(a).each(function (i, m) {
                $("#productList").append("<option value='" + m.id + "'  >" + m.Product_Spec + '-' + m.Product_Name + "</option>")
            })

        },
        error: function (err) {
            console.log(err);
        }
    })
})
//根据商品型号 获取产品参数
$("#productList").change(function () {

    var id = $("#productList").val();
    $("#parameter tbody").empty();
    $("#SKU p").text("");
    $.ajax({
        url: 'http://api.chiccasa.com.cn/ChicERP.asmx/Get_Parameters',
        type: 'post',
        data: "{_id:" + id + "}",//传入的参数
        contentType: "application/json;charset=utf-8",
        dataType: 'json',
        success: function (data) {
            var a = JSON.parse(data.d);
            console.log(a)
            //分组
            var map = {},
                dest = [];
            for (var i = 0; i < a.length; i++) {
                var ai = a[i];

                if (!map[ai.Para_Name]) {
                    dest.push({
                        Is_MasterPara: ai.Is_MasterPara,
                        Para_Name: ai.Para_Name,
                        data: [ai]
                    });
                    map[ai.Para_Name] = ai;
                } else {
                    for (var j = 0; j < dest.length; j++) {
                        var dj = dest[j];
                        if (dj.Para_Name == ai.Para_Name) {
                            dj.data.push(ai);
                            break;
                        }
                    }
                }
            }
            //console.log(dest);

            $(dest).each(function (i, m) {
                var sel = ` <tr>
                                <td>${i + 1}</td>
                                <td>
                                    <input type="text" value="${m.Para_Name}" data-istf="${m.Is_MasterPara}" readonly/>
                                </td>
                                <td>
                                    <select  id="name${i}" onchange="chooseFun(this)">
                                        <option value="" class="active">-请选择参数名-</option>
                                    </select>
                                </td>
                                <td>
                                    <select id="choose${i}">
                                        <option value="">-请选择可选项-</option>
                                    </select>
                                </td>
                            </tr>`;
                $("#parameter tbody").append(sel);

                $(m.data).each(function (a, b) {

                    $("#name" + i + "").append(" <option value=" + b.Para_Value + ">" + b.Para_Value + "</option>")
                })

            })


        },
        error: function (err) {
            console.log(err);
        }
    })
})

//获取可选参数
function chooseFun(_this) {
    //console.log(_this.id);
    var productID = $("#productList").val(),//商品ID
        isTF = $.trim($("#" + _this.id + "").parent().prev().find("input").data("istf")),//是否主参数
        paraName = $.trim($("#" + _this.id + "").parent().prev().find("input").val()),//参数名
        paraVal = $.trim($("#" + _this.id + "").val());//参数值
    $("#SKU p").text("");
    //console.log(productID,isTF,paraName,paraVal);
    $("#" + _this.id + "").parent().next().find("select").empty();
    $.ajax({
        url: 'http://api.chiccasa.com.cn/ChicERP.asmx/Get_Options',
        type: 'post',
        data: JSON.stringify({ _Product_ID: productID, _IsMaster: isTF, _Para_Name: paraName, _Para_Value: paraVal }),//传入的参数
        contentType: "application/json",
        dataType: 'json',
        success: function (data) {
            var a = JSON.parse(data.d);
            //console.log(a)
            $(a).each(function (i, m) {
                $("#" + _this.id + "").parent().next().find("select").append("<option value='" + m.options + "'  >" + m.options + "</option>")
            })

        },
        error: function (err) {
            console.log(err);
        }
    })
}
function getSKU() {
    let productId = $("#productList").val(),//产品id
        trs = document.getElementById("parameter"),
        Arr = [],
        main = '',//主参数
        assist = '',//辅参数
        model = null,//型号
        series = null,//系列
        modelText = $("#model").find("option.active").text();//产品型号-系列(用于页面展示)
    modelText = modelText.split("-");
    model = modelText[0];
    series = modelText[1];
    for (let i = 1; i < trs.rows.length; i++) {
        var parameter = {
            "isTF": trs.rows[i].cells[1].getElementsByTagName("input")[0].getAttribute("data-istf"),
            "name": trs.rows[i].cells[1].getElementsByTagName("input")[0].value + ":" + trs.rows[i].cells[2].getElementsByTagName("select")[0].value + ":" + trs.rows[i].cells[3].getElementsByTagName("select")[0].value
        }
        Arr.push(parameter);
    }
    //console.log(Arr)
    //拼接主辅参数
    $(Arr).each(function (i, m) {
        if (m.isTF) {
            main += m.name + "/";
        } else {
            assist += m.name + "/";
        }
    })
    main = main.substr(0, main.length - 1);
    assist = assist.substr(0, assist.length - 1);
    let params = JSON.stringify({ _Product_ID: productId, _MasterPara: main, _SlavePara: assist });
    console.log(params)
    $.ajax({
        url: "http://api.chiccasa.com.cn/ChicERP.asmx/Get_SKU ",
        type: "post",
        data: params,
        dataType: "json",
        async: true,
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            goods = JSON.parse(data.d);
            if (goods.length > 0) {
                var html = `SKU编号:${goods[0].SKU_No} 描述：${goods[0].SKU_Description}  `;
                $("#SKU p").text(html);
          

            } else {
                alert("暂无当前参数配置商品！");
            }

        },
        error: function (err) {
            console.log(err);
        }
    })
}
//配置模态框
$("#add-dialog").dialog({
    autoOpen: false,
    height: 400,
    width: 960,
    modal: true,
    buttons: {

        "取消": function () { $(this).dialog("close") },
        "确认": function () {
            console.log(goods)
            let Number = parseInt($.trim($("#goodsTab .Number:last").text())) + 1,//序号
                skuID = goods[0].SKU_ID ,//skuID
                number = 1 ,//数量
                normPrice = goods[0].Price_Retail ,//标准零售价
              
                notes = "",//明细备注
                days = goods[0].Delivery,//周期
                predictDate = addDate(new Date().toLocaleDateString(), days),//预计交期
               
                store = [{
                    "序号": Number,
                    "单据ID": billsID,
                    "SKU_ID": skuID,
                    "标准零售价": normPrice,
                    "数量": number,
                    "零售单价": normPrice,
                    "零售小计": normPrice,
                    "明细备注": notes,
                    "交货周期": days,
                    "预计交期": predictDate,
                    "需求日期": predictDate,
                    "默认交期": predictDate,
                }];
            $.ajax({
                url: detailUrl,
                type: 'post',
                data: { lis: JSON.stringify(store)},
                dataType: 'json',
                success: function (data) {
                    console.log(data);
                    if (data.success) {
                        alert(data.msg);
                        window.location.reload();
                        
                       
                    } else {
                        alert(data.msg);
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
           
            $("#add-dialog").dialog("close");
           
          

        }
    }
});
//计算零售小计
function totalMoney(_this, brother) {

    var price = $(_this).val(),
        num = $(_this).parent().siblings(brother).find("input").val(),
        total = null;
    total = price * num;
    $(_this).parent().siblings(".total").find("input").val(total);
    //allTatol();

}
//获取选择文本
function activeText(Jopts) {
    var value = Jopts.val();//这个值就是你获取的值;
    if (value != "") {
        for (var i = 0; i < Jopts.find("option").length; i++) {
            if (value == Jopts.find("option")[i].value) {
                //console.log(Jopts.find("option")[i]);
                if ($(Jopts.find("option")[i]).hasClass("active")) {
                    return;
                } else {
                    $(Jopts.find("option")[i]).siblings().removeClass("active")
                    $(Jopts.find("option")[i]).addClass("active");

                }


                break;
            }
        }
    }
}
//日期计算（用于计算交期）
function addDate(date, days) {
    var d = new Date(date);
    d.setDate(d.getDate() + days);
    var m = d.getMonth() + 1;
    return d.getFullYear() + '-' + m + '-' + d.getDate();
}
//function allTatol() {
//    var allTatol = null;
//    $("#goodsTab .total").each(function (i, m) {
//        let p = parseFloat($(m).find("input").val());
//        allTatol += p;
//    })
//    $("#allTotal").val(allTatol);
//    //console.log(allTatol);

//}