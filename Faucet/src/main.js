import Vue from 'vue';
import App from './App.vue';
import VueResource from 'vue-resource'
import VeeValidate from 'vee-validate';
import VueQrcode from '@xkeshi/vue-qrcode';
import VueToastr from '@deveodk/vue-toastr'


Vue.config.productionTip = false;
Vue.use(VueResource);
Vue.use(VeeValidate);
Vue.use(VueToastr,{
    defaultPosition: 'toast-top-right',
    defaultTimeout: 5000,
    clickClose: true
});
Vue.component('qrcode', VueQrcode);

new Vue({
  render: h => h(App)
}).$mount('#app');
