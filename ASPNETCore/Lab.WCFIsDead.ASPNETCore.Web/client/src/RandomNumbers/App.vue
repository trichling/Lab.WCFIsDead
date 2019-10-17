<template>
  <div id="app">
    <HelloWorld msg="Random numbers"/>
  </div>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import HelloWorld from '../components/HelloWorld.vue';
    import * as signalR from '@aspnet/signalr';
    import { Guid } from 'guid-typescript';

@Component({
  components: {
    HelloWorld,
  },
})
export default class App extends Vue {

    created () {
        this.useSignalR();
        //this.useServerSendEvents();
    }

    private useServerSendEvents(): void {
        let sequence1 = new EventSource(`/randomNumbers/generate?requestId=${Guid.create()}&count=10&delay=1000`);
        sequence1.addEventListener('connected', () => { console.log("Sqeuence 1 connected") });
        sequence1.addEventListener('end-of-stream', () => { sequence1.close() });
        sequence1.addEventListener('message', evt => {
            let message = JSON.parse(evt.data);
            console.log(message);
        });

        let sequence2 = new EventSource(`/randomNumbers/generate?requestId=${Guid.create()}&count=5&delay=2000`);
        sequence2.addEventListener('connected', () => { console.log("Sqeuence 2 connected") });
        sequence2.addEventListener('end-of-stream', () => { sequence2.close() });
        sequence2.addEventListener('message', evt => {
            let message = JSON.parse(evt.data);
            console.log(message);
        });
    }

    private useSignalR(): void {
         const connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/randomNumbers")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.start().then(() => {
            connection.stream("GetRandomNumbers", Guid.create().toString(), 10, 1000)
            .subscribe(new RandomNumberLogger());

            connection.stream("GetRandomNumbers", Guid.create().toString(), 5, 2000)
                .subscribe({
                    next:  item => { console.log(item.requestId + ": " + item.randomNumber); },
                    complete : () => { },
                    error: err => { }
                })
        });
    }
}

    class RandomNumberLogger implements signalR.IStreamSubscriber<RandomNumberResult>{

        next = (item: RandomNumberResult) => { console.log(item.requestId + ": " + item.randomNumber); }
        complete = () => { }
        error = (err: any) => { }

    }

    class RandomNumberResult {

        requestId: string = "";
        randomNumber: number = 0;

    }
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
