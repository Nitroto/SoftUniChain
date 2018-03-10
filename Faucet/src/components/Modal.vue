<template>
    <transition name="modal">
        <div class="modal-mask">
            <div class="modal-wrapper">
                <div class="modal-container">

                    <div class="modal-header has-text-centered">
                        <slot name="header">
                            <strong>Donate to Chuchurka</strong>
                        </slot>
                    </div>

                    <div class="modal-body has-text-centered">
                        <slot name="body">
                            <qrcode v-bind:value="address"
                                    :options="{ size: 200, level: 'H', backgroundAlpha: 0.5, padding: 25}"></qrcode>
                            <strong>{{ address }}</strong>
                        </slot>
                    </div>

                    <div class="modal-footer has-text-centered">
                        <slot name="footer">
                            <button class="button is-primary is-focused" @click="$emit('close')">Done</button>
                        </slot>
                    </div>
                </div>
            </div>
        </div>
    </transition>
</template>

<script>
    export default {
        name: "modal",
        props: ['address'],
        data: function () {
            return {
                showModal: false
            }
        }
    }
</script>

<style scoped lang="sass">
    .modal-mask
        position: fixed
        z-index: 9998
        top: 0
        left: 0
        width: 100%
        height: 100%
        background-color: rgba(0, 0, 0, .5)
        display: table
        transition: opacity .3s ease

        .modal-wrapper
            display: table-cell
            vertical-align: middle

        .modal-container
            width: 450px
            height: 365px
            margin: 0px auto
            padding: 20px 30px
            background-color: #fff
            border-radius: 2px
            box-shadow: 0 2px 8px rgba(0, 0, 0, .33)
            transition: all .3s ease
            font-family: Helvetica, Arial, sans-serif

        .modal-header h3
            margin-top: 0
            color: #42b983

        .modal-body
            margin: 20px 0

        .modal-default-button
            float: right

        /*
         * The following styles are auto-applied to elements with
         * transition="modal" when their visibility is toggled
         * by Vue.js.
         *
         * You can easily play with the modal transition by editing
         * these styles.
         */

        .modal-enter
            opacity: 0

        .modal-leave-active
            opacity: 0

        .modal-enter .modal-container,
        .modal-leave-active .modal-container
            -webkit-transform: scale(1.1)
            transform: scale(1.1)
</style>
