﻿/*jshint esversion: 6 */
const dateFormatValue = 'YYYY-MM-DD';
import Toasted from 'vue-toasted';

import Loading from 'vue-loading-overlay';
// Import stylesheet
import 'vue-loading-overlay/dist/vue-loading.css';
import '../css/site.css';
import '../css/sitestyle.scss';


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
    chosenFeeling: 5,
    chosenDate: new Date().toDateInputValue(),
    chosenActivity: '',
    chosenComment: '',
    chosenId: 0,
    logEntries: [],
    fullPage: true,
    logEditorOpen: false,
    selectedLog: {},
    currentLogPage: 1
};

var app = new Vue({
    el: '#app',
    data: data,
    filters: {
        dateFormat: function(value) {
            return moment(value).format(dateFormatValue);
        },
        sleepFormat: function(value) {
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
                }).catch(() => {
                    alert('uh-oh, something went wrong');
                });
        },
        postLog: function() {
            var method = this.chosenId !== 0 ? 'PUT' : 'POST';
            const url = this.chosenId !== 0 ? '/api/log/' + this.chosenId : '/api/log';

            let loader = this.$loading.show({
                // Optional parameters
                container: this.fullPage ? null : this.$refs.formContainer,
                canCancel: true,
                onCancel: this.onCancel,
            });

            let data = {
                feeling: this.chosenFeeling,
                date: this.chosenDate,
                comment: this.chosenComment,
                activity: this.chosenActivity,
                //can be removed
                id: this.chosenId
            }
            let fetchData = {
                method: method,
                body: JSON.stringify(data),
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                },
            }
            fetch(url, fetchData)
                .then(response => {
                    if (!response.ok) {
                        throw Error(response.statusText);
                    }

                    return response;
                })
                .then(response => {
                    this.chosenComment = null;
                    this.chosenActivity = null;
                    this.chosenFeeling = null;
                    this.chosenDate = null;
                    this.chosenId = 0;
                    var actionName = method === "POST" ? 'added' : 'updated';
                    this.$toasted.show('Log ' + actionName + '!');
                    this.resetLogEdit();

                    setTimeout(() => {
                        loader.hide();
                        this.getEntries();
                    }, 500);
                }).catch((error) => {
                    alert('Uh-oh, something went wrong: ' + error);
                    loader.hide();
                });
        },
        editLog: function(log) {
            this.chosenComment = log.comment;
            this.chosenActivity = log.activity;
            this.chosenDate = moment(log.date).format(dateFormatValue);
            this.chosenId = log.id;
            this.chosenFeeling = log.feeling;
            this.logEditorOpen = true;

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
            this.chosenFeeling = 5;
            this.chosenDate = new Date().toDateInputValue();
            this.chosenActivity = '';
            this.chosenComment = '';
            this.chosenId = 0;
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
        }
    },
    created: function() {
        this.getEntries();
    }
});