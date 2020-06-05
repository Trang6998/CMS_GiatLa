import Vue from 'vue'
import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'

import en from './CalendarApp/locales/en'
import vi from './CalendarApp/locales/vi'
import DaySpanVuetify from './CalendarApp/plugin'
import 'material-design-icons-iconfont/dist/material-design-icons.css'

Vue.use(Vuetify, {
    iconfont: 'md'
})
Vue.use(DaySpanVuetify, {
    data:
    {
        locales: { vi, en }
    },
    methods: {
        getDefaultEventColor: () => '#1976d2'
    }
});
