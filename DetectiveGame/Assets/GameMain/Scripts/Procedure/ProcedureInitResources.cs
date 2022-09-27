using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework;
using UnityGameFramework.Runtime;
using GameFramework.Resource;

namespace DetectiveGame
{
    public class ProcedureInitResources : ProcedureBase
    {
        private bool initResourceComplete = false;
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            initResourceComplete = false;

            GameEntry.Resource.InitResources(OnInitResourceComplete);
        }

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (initResourceComplete)
                ChangeState<ProcedurePreload>(procedureOwner);
            
        }

        private void OnInitResourceComplete()
        {
            initResourceComplete = true;
            Log.Info("Init resources complete.");
        }
    }
}