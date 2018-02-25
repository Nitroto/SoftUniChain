import Vue from 'vue';
import App from './App.vue';
import VeeValidate from 'vee-validate';
import VueQrcode from '@xkeshi/vue-qrcode';
import Toastr from 'vue-toastr';


Vue.config.productionTip = false;
Vue.use(VeeValidate);
Vue.component('qrcode', VueQrcode);
Vue.use(Toastr);

new Vue({
  render: h => h(App)
}).$mount('#app');
