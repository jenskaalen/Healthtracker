!function(t){var e={};function o(n){if(e[n])return e[n].exports;var i=e[n]={i:n,l:!1,exports:{}};return t[n].call(i.exports,i,i.exports,o),i.l=!0,i.exports}o.m=t,o.c=e,o.d=function(t,e,n){o.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:n})},o.r=function(t){"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},o.t=function(t,e){if(1&e&&(t=o(t)),8&e)return t;if(4&e&&"object"==typeof t&&t&&t.__esModule)return t;var n=Object.create(null);if(o.r(n),Object.defineProperty(n,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var i in t)o.d(n,i,function(e){return t[e]}.bind(null,i));return n},o.n=function(t){var e=t&&t.__esModule?function(){return t.default}:function(){return t};return o.d(e,"a",e),e},o.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},o.p="",o(o.s=1)}([function(t,e,o){
/*!
 * Toastify js 1.5.0
 * https://github.com/apvarun/toastify-js
 * @license MIT licensed
 *
 * Copyright (C) 2018 Varun A P
 */
var n,i;n=this,i=function(t){var e=function(t){return new e.lib.init(t)};function o(t,e){return!(!t||"string"!=typeof e)&&!!(t.className&&t.className.trim().split(/\s+/gi).indexOf(e)>-1)}return e.lib=e.prototype={toastify:"1.5.0",constructor:e,init:function(t){return t||(t={}),this.options={},this.toastElement=null,this.options.text=t.text||"Hi there!",this.options.duration=t.duration||3e3,this.options.selector=t.selector,this.options.callback=t.callback||function(){},this.options.destination=t.destination,this.options.newWindow=t.newWindow||!1,this.options.close=t.close||!1,this.options.gravity="bottom"==t.gravity?"toastify-bottom":"toastify-top",this.options.positionLeft=t.positionLeft||!1,this.options.backgroundColor=t.backgroundColor,this.options.avatar=t.avatar||"",this.options.className=t.className||"",this.options.stopOnFocus=t.stopOnFocus||!0,this},buildToast:function(){if(!this.options)throw"Toastify is not initialized";var t=document.createElement("div");if(t.className="toastify on "+this.options.className,!0===this.options.positionLeft?t.className+=" toastify-left":t.className+=" toastify-right",t.className+=" "+this.options.gravity,this.options.backgroundColor&&(t.style.background=this.options.backgroundColor),t.innerHTML=this.options.text,""!==this.options.avatar){var e=document.createElement("img");e.src=this.options.avatar,e.className="toastify-avatar",!0===this.options.positionLeft?t.appendChild(e):t.insertAdjacentElement("beforeend",e)}if(!0===this.options.close){var o=document.createElement("span");if(o.innerHTML="&#10006;",o.className="toast-close",o.addEventListener("click",function(t){t.stopPropagation(),this.removeElement(t.target.parentElement),window.clearTimeout(t.target.parentElement.timeOutValue)}.bind(this)),this.options.stopOnFocus&&this.options.duration>0){const e=this;t.addEventListener("mouseover",function(e){window.clearTimeout(t.timeOutValue)}),t.addEventListener("mouseleave",function(){t.timeOutValue=window.setTimeout(function(){e.removeElement(t)},e.options.duration)})}var n=window.innerWidth>0?window.innerWidth:screen.width;!0===this.options.positionLeft&&n>360?t.insertAdjacentElement("afterbegin",o):t.appendChild(o)}return void 0!==this.options.destination&&t.addEventListener("click",function(t){t.stopPropagation(),!0===this.options.newWindow?window.open(this.options.destination,"_blank"):window.location=this.options.destination}.bind(this)),t},showToast:function(){var t;if(this.toastElement=this.buildToast(),!(t=void 0===this.options.selector?document.body:document.getElementById(this.options.selector)))throw"Root element is not defined";return t.insertBefore(this.toastElement,t.firstChild),e.reposition(),this.options.duration>0&&(this.toastElement.timeOutValue=window.setTimeout(function(){this.removeElement(this.toastElement)}.bind(this),this.options.duration)),this},hideToast:function(){this.toastElement.timeOutValue&&clearTimeout(this.toastElement.timeOutValue),this.removeElement(this.toastElement)},removeElement:function(t){t.className=t.className.replace(" on",""),window.setTimeout(function(){t.parentNode.removeChild(t),this.options.callback.call(t),e.reposition()}.bind(this),400)}},e.reposition=function(){for(var t,e={top:15,bottom:15},n={top:15,bottom:15},i={top:15,bottom:15},s=document.getElementsByClassName("toastify"),a=0;a<s.length;a++){t=!0===o(s[a],"toastify-top")?"toastify-top":"toastify-bottom";var r=s[a].offsetHeight;t=t.substr(9,t.length-1);(window.innerWidth>0?window.innerWidth:screen.width)<=360?(s[a].style[t]=i[t]+"px",i[t]+=r+15):!0===o(s[a],"toastify-left")?(s[a].style[t]=e[t]+"px",e[t]+=r+15):(s[a].style[t]=n[t]+"px",n[t]+=r+15)}return this},e.lib.init.prototype=e.lib,e},t.exports?t.exports=i():n.Toastify=i()},function(t,e,o){"use strict";o.r(e);var n=o(0);Date.prototype.toDateInputValue=function(){var t=new Date(this);return t.setMinutes(this.getMinutes()-this.getTimezoneOffset()),t.toJSON().slice(0,10)};var i={message:"Hello Vue!",inputText:"write here",messages:["wee","woo"],chosenFeeling:5,chosenDate:(new Date).toDateInputValue(),chosenActivity:"",chosenComment:"",chosenId:0,logEntries:[]};new Vue({el:"#app",data:i,filters:{dateFormat:function(t){return moment(t).format("YYYY-MM-DD")}},methods:{getEntries:function(){fetch("/api/log").then(t=>{if(!t.ok)throw Error(t.statusText);return t.json()}).then(t=>{this.logEntries=t}).catch(()=>{alert("uh-oh, something went wrong")})},postMessage:function(){this.messages.push(this.message),this.message=""},postLog:function(){var t=0!==this.chosenId?"PUT":"POST";const e=0!==this.chosenId?"/api/log/"+this.chosenId:"/api/log";let o={feeling:this.chosenFeeling,date:this.chosenDate,comment:this.chosenComment,activity:this.chosenActivity,id:this.chosenId},n={method:t,body:JSON.stringify(o),headers:{"Content-Type":"application/json; charset=utf-8"}};fetch(e,n).then(t=>{if(!t.ok)throw Error(t.statusText);return t}).then(t=>{this.chosenComment=null,this.chosenActivity=null,this.chosenFeeling=null,this.chosenDate=null,this.chosenId=0,this.getEntries()}).catch(t=>{alert("uh-oh, something went wrong: "+t)})},editLog:function(t){this.chosenComment=t.comment,this.chosenActivity=t.activity,this.chosenDate=moment(t.date).format("YYYY-MM-DD"),this.chosenId=t.id,this.chosenFeeling=t.feeling,document.getElementById("logForm").scrollIntoView(),Object(n.Toastify)({text:"Log updated"})},deleteLog:function(t){var e="/api/log/"+t.id;fetch(e,{method:"DELETE",headers:{"Content-Type":"application/json; charset=utf-8"}}).then(t=>{console.log("deleted log")}).catch(t=>{console.log(t),alert("uh-oh, something went wrong")})}},computed:{orderedLogs:function(){return _.orderBy(this.logEntries,"date")}}})}]);