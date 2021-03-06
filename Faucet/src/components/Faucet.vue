<template>
    <div class="columns">
        <div class="column"></div>
        <div class="column is-three-quarters">
            <div class="faucet">
                <section class="section">
                    <div class="has-text-centered">
                        <h1><strong>Balance: {{ balance | currency('SUC', 0, { symbolOnLeft: false,
                            spaceBetweenAmountAndSymbol: true }) }}</strong></h1>
                    </div>

                    <div class="container">
                        <form id="form" @submit.prevent="validateBeforeSubmit" method="post" novalidate="true"
                              action="">

                            <div class="field">
                                <label class="label">Your address</label>
                                <p class="control has-icon has-icons-left">
                                    <input name="recipient"
                                           v-model="recipient"
                                           v-validate="'required|regex:^[a-f0-9]{40}$'"
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
                                        <input class="button is-info is-fullwidth"
                                               type="submit"
                                               value="Get coins!"
                                               :disabled="(!(isVerify && !errors.has('nodeUrl') && !errors.has('recipient')))">
                                    </p>
                                </div>
                            </div>
                        </form>
                    </div>
                </section>

                <table class="table is-striped is-fullwidth" v-for="transaction of transactions"
                       :key="transaction.transactionHash">
                    <thead>
                    <tr v-bind:bgcolor="transaction.transferSuccessful ? '#d0f0c0' : '#ffd9e0'">
                        <th colspan="2"><abbr title="Hashes"></abbr>
                            <a v-bind:href="'http://localhost:3000/transactions/' +transaction.transactionHash">{{transaction.transactionHash}}</a>
                        </th>
                        <th><abbr title="Date Created"></abbr>{{transaction.dateCreated | moment("dddd, MMMM Do YYYY")}}
                        </th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td>
                            <a v-bind:href="'http://localhost:3000/address/'+ transaction.from">{{transaction.from}}</a>
                        </td>
                        <td>
                            <span class='icon'>
                                <i class='fas fa-arrow-right successful'></i>
                            </span>
                        </td>
                        <td>
                            <a v-bind:href="'http://localhost:3000/address/' + transaction.to">{{transaction.to}}</a>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Amount:</td>
                        <td>{{transaction.value | currency('&mu;SUC', 0, { symbolOnLeft: false,
                            spaceBetweenAmountAndSymbol: true })}}
                        </td>
                    </tr>
                    <tr>
                        <td colSpan="2">Fee:</td>
                        <td>{{transaction.fee | currency('&mu;SUC', 0, { symbolOnLeft: false,
                            spaceBetweenAmountAndSymbol: true })}}
                        </td>
                    </tr>
                    </tbody>
                </table>
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
        created: function () {
            let url = this.nodeUrl + '/api/address/' + this.address;
            this.$http.get(url, {
                headers: {
                    'Content-Type': 'application/json',
                    'Access-Control-Allow-Origin': '*'
                }
            }).then(result => {
                this.balance = result.body.amount / 1000000;
            }, () => {
                this.$toastr('error', 'There is some connection problem.', 'Error');
            });

        },
        data: function () {
            return {
                balance: 0,
                siteKey: '6LezVkgUAAAAAEDM9ZQyuYB_gAepXJoX0Jg-UpFx',
                recipient: '',
                nodeUrl: 'http://localhost:5000',
                isVerify: false,
                transactions: []
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
                }).then(result => {
                    this.$toastr('success', 'You received 1 SUC.', 'Success');
                    this.transactions.push(result.body);
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
