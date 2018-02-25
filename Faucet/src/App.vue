<template>
    <div id="app">
        <nav class="navbar is-primary level is-transparent has-shadow">
            <p class="level-item has-text-centered">
                <span class="icon is-large">
                    <i class="fab fa-2x fa-connectdevelop"></i>
                </span>
                SoftUni Chain - Faucet
            </p>
            <div class="level-right ">
                <button id="show-modal" @click="showModal = true" class="button level-right is-info is-outlined">
                    <span class="icon"><i class="fas fa-dollar-sign"></i></span>
                    <span>Donate?</span>
                </button>
            </div>
        </nav>
        <faucet :privateKey="privateKey" :publicKey="publicKey" :address="address" />

        <modal v-if="showModal" @close="showModal = false" :address="address"></modal>

        <footer class="footer is-primary">
            <div class="container">
                <div class="content has-text-centered">
                    <p>&copy; 2018 <strong>SoftUni Chain Faucet</strong> - educational blockchain testing network</p>
                </div>
            </div>
        </footer>
    </div>
</template>

<script>
    import Faucet from './components/Faucet.vue';
    import Modal from './components/Modal.vue';

    export default {
        name: 'app',
        components: {
            Faucet,
            Modal
        },
        data: function () {
            return{
                privateKey: 'f827e168cc789351ed3589b1ec4ad58551319eef08ff5261a473be3068be4cc3',
                publicKey: '5b209167be272fb95fc86a8ae6c74035d8775afe5ad3c4b0cb99a4a25bc8b2f31',
                address: 'bee3f694bf0fbf9556273e85d43f2e521d24835e',
                showModal: false
            }
        }

    }
</script>

<style lang="sass">
    @import "../node_modules/bulma/bulma.sass"
    @import "mq"

    nav p
        font-weight: bold
        font-size: larger

    footer
        background-color: $primary !important
        color: white

    footer div p strong
        color: white

    #show-modal
        margin-right: .8rem

</style>
