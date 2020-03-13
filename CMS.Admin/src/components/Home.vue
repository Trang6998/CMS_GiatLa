<template>
    <div class="home">
        <!--<v-card>
            <v-sheet class="v-sheet--offset mx-auto"
                     color="cyan"
                     elevation="12"
                     max-width="calc(100% - 32px)">
                <v-sparkline :labels="labels"
                             :value="value"
                             color="white"
                             line-width="2"
                             padding="16"></v-sparkline>
            </v-sheet>
        </v-card>-->
    </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    export default Vue.extend({
        data() {
            return {
                labels: [
                    '1',
                    '2',
                    '3',
                    '4',
                    '5',
                    '6',
                    '7',
                    '8',
                ],
                value: [
                    200,
                    675,
                    410,
                    390,
                    310,
                    460,
                    250,
                    240,
                ],
            }
        }
    })
</script>
