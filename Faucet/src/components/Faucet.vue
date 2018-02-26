<template>
    <div class="columns">
        <div class="column"></div>
        <div class="column is-three-quarters">
            <div class="faucet">
                <section class="section">
                    <div class="has-text-centered">
                        <h1><strong>Balance: {{ balance }} SUC</strong></h1>
                    </div>

                    <div class="container">
                        <form id="form" @submit.prevent="validateBeforeSubmit" method="post" novalidate="true"
                              action="">

                            <div class="field">
                                <label class="label">Your address</label>
                                <p class="control has-icon has-icons-left">
                                    <input name="recipient"
                                           v-model="recipient"
                                           v-validate="'required|regex:^[A-Za-z0-9]{40}$'"
                                           :class="{'input': true, 'is-danger': errors.has('recipient') }"
                                           type="text"
                                           placeholder="Your blockchain address">
                                    <span v-show="!errors.has('recipient')" class="icon is-small"><i
                                            class="fab fa-btc"></i></span>
                                    <span v-show="errors.has('recipient')" class="icon is-small"><i
                                            class="fas fa-exclamation-triangle"></i></span>
                                    <span v-show="errors.has('recipient')" class="help is-danger">{{ errors.first('recipient') }}</span>
                                </p>
                            </div>


                            <div class="field">
                                <label class="label">Node URL</label>
                                <p class="control has-icon has-icons-left">
                                    <input name="nodeUrl"
                                           v-model="nodeUrl"
                                           v-validate="'required'"
                                           :class="{'input': true, 'is-danger': errors.has('nodeUrl') }"
                                           type="text"
                                           placeholder="Node URL">
                                    <span v-show="!errors.has('nodeUrl')" class="icon is-small is-left"><i
                                            class="fas fa-link"></i></span>
                                    <span v-show="errors.has('nodeUrl')" class="icon is-small"><i
                                            class="fas fa-exclamation-triangle"></i></span>
                                    <span v-show="errors.has('nodeUrl')" class="help is-danger">{{ errors.first('nodeUrl') }}</span>
                                </p>
                            </div>

                            <vue-recaptcha
                                    ref="recaptcha"
                                    @verify="isVerify = true"
                                    @expired="isVerify = false"
                                    :sitekey="siteKey">
                            </vue-recaptcha>

                            <br/>

                            <div class="field is-grouped">
                                <div class="control">
                                    <p>
                                        <input class="button is-info"
                                               type="submit"
                                               value="Get coins!"
                                               :disabled="(!(isVerify && !errors.has('nodeUrl') && !errors.has('recipient')))">
                                    </p>
                                </div>
                            </div>
                        </form>
                    </div>
                </section>
            </div>
        </div>
        <div class="column"></div>
    </div>
</template>

<script>
    import VueRecaptcha from 'vue-recaptcha';

    import * as elliptic from 'elliptic';
    import * as hashes from 'jshashes';
    import * as utf8 from 'utf8';

    export default {
        name: "faucet",
        props: ['privateKey', 'publicKey', 'address'],
        components: {
            VueRecaptcha
        },
        data: function () {
            return {
                balance: 0,
                siteKey: '6LezVkgUAAAAAEDM9ZQyuYB_gAepXJoX0Jg-UpFx',
                recipient: '',
                nodeUrl: 'http://loclhost:5000',
                isVerify: false,
            };
        },
        methods: {
            validateBeforeSubmit: function () {
                this.$validator.validateAll().then((result) => {
                    if (result) {
                        this.signTransaction();
                    }
                });
            },

            signTransaction: function () {
                let transactionPayLoad = {
                    'from': this.address,
                    'to': this.recipient,
                    'senderPublicKey': this.publicKey,
                    'value': 1000000,
                    'fee': 20,
                    'dateCreated': new Date().toISOString()
                };

                let transactionPayLoadAsString = JSON.stringify(transactionPayLoad).toString();
                let transactionPayloadHash = new hashes.SHA256().hex(utf8.encode(transactionPayLoadAsString));
                let privateKey = new elliptic.ec('secp256k1').keyFromPrivate(this.privateKey);
                let transactionSignature = privateKey.sign(transactionPayloadHash);
                let senderSignature = [transactionSignature.r.toString(16), transactionSignature.s.toString(16)];
                let data = transactionPayLoad;
                data['senderSignature'] = senderSignature;
                this.sendTransaction(data);
            },

            sendTransaction(data) {
                let url = this.nodeUrl + '/api/transactions';
                this.$http.post(url, data, {
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*'
                    }
                }).then(() => {
                    this.$toastr('success', 'Success msg', 'Success');
                    this.recipient = '';
                    this.resetForm();
                }, () => {
                    this.$toastr('error', 'Error msg', 'Error');
                    this.resetForm();
                });
            },

            resetForm() {
                this.isVerify = false;
                this.$refs.recaptcha.reset();
                this.$validator.reset()
            }
        }
    }
</script>

<style lang="sass">
    @import "../mq"

</style>