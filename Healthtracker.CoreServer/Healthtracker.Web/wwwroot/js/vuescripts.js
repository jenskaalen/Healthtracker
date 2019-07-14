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
        date: new Date().toDateInputValue(),
        activities: [],
        supplements: []
    },
    notificationHub: signalrHub,
    filter: '',
    newActivity: '',
    newSupp: '',
    activitySuggestions: [],
    supplementSuggestions: []
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
        addNewLog() {
            this.selectedLog = {
                date: new Date().toDateInputValue(),
                feeling: _.orderBy(this.logEntries, 'date', 'desc')[0].feeling,
                activities: [],
                supplements: []
            };

            this.logEditorOpen = true;
        },
        removeFromActivities: function(act) {
            console.log('fafa');
            this.selectedLog.activities.splice(this.selectedLog.activities.indexOf(act), 1);
        },
        removeFromSupplements: function(supp) {
            this.selectedLog.supplements.splice(this.selectedLog.supplements.indexOf(supp), 1);
        },
        addActivity: function(act) {
            if (!this.selectedLog.activities) {
                this.selectedLog.activities = [];
            }


            this.selectedLog.activities.push({
                integrationSource: "User",
                name: act
            });

            if (this.newActivity === act) {
                this.newActivity = null;
            }
        },
        addSupplement: function(supp) {
            if (!this.selectedLog.supplements) {
                this.selectedLog.supplements = [];
            }

            this.selectedLog.supplements.push(supp);

            if (this.newSupp === supp) {
                this.newSupp = null;
            }
        },
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

                }).catch(() => {
                    alert('uh-oh, something went wrong');
                });
        },
        postLog: function() {
            var method = this.selectedLog.id ? 'PUT' : 'POST';
            const url = this.selectedLog.id ? '/api/log/' + this.selectedLog.id : '/api/log';

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
            return _.orderBy(this.logEntries, 'date', 'desc');
        },
        suggestionNotInActivities: function() {
            return this.activitySuggestions.filter(val => !this.selectedLog.activities || this.selectedLog.activities.map(x => x.name).indexOf(val) < 0);
        },
        suggestionNotInSupplements: function() {
            return this.supplementSuggestions.filter(val => !this.selectedLog.supplements || this.selectedLog.supplements.map(x => x).indexOf(val) < 0);
        }
    },
    created: function() {
        this.getEntries();

        let toaster = this.$toasted;
        this.notificationHub.on("ReceiveMessage", (user, message) => {
            toaster.show(message);
        });

        fetch('/api/LogActivity/suggestions')
            .then(response => {
                if (!response.ok) {
                    throw Error(response.statusText);
                }
                return response.json();
            })
            .then(response => {
                console.log(response);
                this.activitySuggestions = response;

            }).catch(() => {
                alert('uh-oh, something went wrong');
            });

        fetch('/api/Supplement/suggestions')
            .then(response => {
                if (!response.ok) {
                    throw Error(response.statusText);
                }
                return response.json();
            })
            .then(response => {
                console.log(response);
                this.supplementSuggestions = response;

            }).catch(() => {
                alert('uh-oh, something went wrong');
            });


        this.notificationHub.start();
    }
});