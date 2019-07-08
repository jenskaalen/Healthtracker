/*jshint esversion: 6 */
const dateFormatValue = 'YYYY-MM-DD';
import Toasted from 'vue-toasted';
const signalR = require('@aspnet/signalr');
import Loading from 'vue-loading-overlay';
// Import stylesheet
import 'vue-loading-overlay/dist/vue-loading.css';
import '../css/site.css';
import '../css/sitestyle.scss';

let signalrHub = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

Date.prototype.toDateInputValue = (function() {
    var local = new Date(this);
    local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
    return local.toJSON().slice(0, 10);
});


Vue.use(Toasted, { position: 'bottom-right', duration: 3000 })
Vue.use(Loading, { color: '#209cee' });

var data = {
    message: 'Hello Vue!',
    inputText: 'write here',
    messages: ['wee', 'woo'],
    logEntries: [],
    fullPage: true,
    logEditorOpen: false,
    currentLogPage: 1,
    selectedLog: {
        date: new Date().toDateInputValue()
    },
    notificationHub: signalrHub,
    filter: ''
};

var app = new Vue({
    el: '#app',
    data: data,
    filters: {
        dateFormat: function(value) {
            return moment(value).format(dateFormatValue);
        },
        sleepFormat: function(value) {
            //rounds decimals
            return value.toFixed(1);
        }
    },
    methods: {
        getEntries: function() {
            let loader = this.$loading.show({
                // Optional parameters
                container: this.fullPage ? null : this.$refs.formContainer,
                canCancel: true,
                onCancel: this.onCancel,
            });

            const url = '/api/log/page/' + this.currentLogPage;
            fetch(url)
                .then(response => {
                    if (!response.ok) {
                        throw Error(response.statusText);
                    }
                    return response.json();
                })
                .then(response => {
                    this.logEntries = response;
                    loader.hide();

                    // signalrHub.invoke("SendMessage", "user123", "new page").catch(function(err) {
                    //     return console.error(err.toString());
                    // });

                }).catch(() => {
                    alert('uh-oh, something went wrong');
                });
        },
        postLog: function() {
            var method = this.selectedLog.id !== null ? 'PUT' : 'POST';
            const url = this.selectedLog.id !== null ? '/api/log/' + this.selectedLog.id : '/api/log';

            let loader = this.$loading.show({
                // Optional parameters
                container: this.fullPage ? null : this.$refs.formContainer,
                canCancel: true,
                onCancel: this.onCancel,
            });

            console.log(this.selectedLog);

            let fetchData = {
                method: method,
                body: JSON.stringify(this.selectedLog),
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                },
            }
            fetch(url, fetchData)
                .then(response => {
                    if (!response.ok) {
                        throw Error(response.statusText);
                    }

                    return response.json();
                })
                .then(response => {
                    var actionName = method === "POST" ? 'added' : 'updated';
                    this.$toasted.show('Log ' + actionName + '!');
                    this.resetLogEdit();

                    if (method === "POST") {
                        this.logEntries.push(response);
                    }

                    setTimeout(() => {
                        loader.hide();
                        // this.getEntries();
                    }, 500);
                }).catch((error) => {
                    alert('Uh-oh, something went wrong: ' + error);
                    loader.hide();
                });
        },
        editLog: function(log) {
            this.logEditorOpen = true;
            this.selectedLog = log;

            setTimeout(() => {

                var elmnt = document.getElementById("logForm");
                elmnt.scrollIntoView();
            }, 500);
        },
        deleteLog: function(log) {
            var url = '/api/log/' + log.id;
            let fetchData = {
                method: 'DELETE',
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                }
            }

            //TODO: remove deleted item from the table
            fetch(url, fetchData)
                .then(response => {
                    // data.logEntries = data.logEntries.filter(item => item !== log);
                    console.log('deleted log');
                    this.$toasted.show('Log deleted!');
                    this.getEntries();
                }).catch((error) => {
                    console.log(error);
                    alert('uh-oh, something went wrong');
                });
        },
        resetLogEdit: function() {
            this.selectedLog = {
                date: new Date().toDateInputValue(),
                feeling: _.orderBy(this.logEntries, 'date', 'desc')[0].feeling
            };
            this.logEditorOpen = false;
        },
        nextPage: function() {
            this.currentLogPage = this.currentLogPage + 1;
            this.getEntries();
        },
        previousPage: function() {
            if (this.currentLogPage <= 1) {
                return;
            }

            this.currentLogPage -= 1;
            this.getEntries();
        }
    },
    computed: {
        orderedLogs: function() {
            //var logs = this.logEntries.filter(log => log.comment.indexOf(this.filter) > 0)

            return _.orderBy(this.logEntries, 'date', 'desc');
        }
    },
    created: function() {
        this.getEntries();
        // let toasty = this.$toasted;


        let toastfuck = this.$toasted;
        // let that = this;
        this.notificationHub.on("ReceiveMessage", (user, message) => {
            toastfuck.show(message);
            // that.getEntries();
        });

        this.notificationHub.start();
    }
});