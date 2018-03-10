import Vue from 'vue';
import App from './App.vue';
import VueResource from 'vue-resource';
import VeeValidate from 'vee-validate';
import VueQrcode from '@xkeshi/vue-qrcode';
import VueToastr from '@deveodk/vue-toastr';
import Vue2Filters from 'vue2-filters';
import VueMoment from 'vue-moment';


Vue.config.productionTip = false;
Vue.use(VueResource);
Vue.use(VeeValidate);
Vue.use(VueToastr, {
    defaultPosition: 'toast-top-right',
    defaultTimeout: 5000,
    clickClose: true
});
Vue.use(VueMoment);
Vue.component('qrcode', VueQrcode);
Vue.use(Vue2Filters);

new Vue({
    render: h => h(App)
}).$mount('#app');
